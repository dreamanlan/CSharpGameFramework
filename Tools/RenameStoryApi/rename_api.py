#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Rename story API / skill trigger names to snake_case across both repos.

Usage:
    py -3 rename_api.py scan     # extract registrations, build BuildLog/rename_map.tsv
    py -3 rename_api.py apply    # rewrite sources + DSL + docs using the map
    py -3 rename_api.py verify   # static check: no old names, DSL calls resolve

The map is a reviewable artifact. No name is guessed at apply time: scan fails
loudly on any identifier whose word boundaries cannot be resolved.
"""
import os
import re
import sys
import collections

ROOTS = {
    'cs': r'd:\GitHub\CSharpGameFramework',
    'demo': r'd:\GitHub\GameDemo',
}
OUT = os.path.join(ROOTS['cs'], 'BuildLog', 'rename_map.tsv')

# Registration calls whose first string literal is the public DSL name.
REG = re.compile(
    r'\b(?:Register\w*|AddTrigger|RegisterTrigger)\s*\(\s*"([A-Za-z_][A-Za-z0-9_]*)"'
)
# Skill trigger factories use a different API name in this codebase.
TRIGGER = re.compile(
    r'\bRegisterTrigerFactory\s*\(\s*"([A-Za-z_][A-Za-z0-9_]*)"'
)

TEXT_EXT = {'.cs', '.dsl', '.py', '.txt', '.md', '.bat', '.cmd', '.json', '.yml', '.yaml'}
SKIP_DIRS = {'.git', '.vs', '.idea', 'Library', 'Temp', 'obj', 'node_modules', '__pycache__'}
SELF = os.path.normcase(os.path.abspath(__file__))


def is_self(path):
    return os.path.normcase(os.path.abspath(path)) == SELF

# Words that count as one independent token. Abbreviations and digits are
# separate tokens by design (getpositionx -> get_position_x, trace2 -> trace_2).
WORDS = set("""
a about abs active actor add after ai all allow alpha also alt anim animation any aoe api append apply
area arg args array as at attack auto avatar axis
back bar base batch before begin behind blackboard bool box break buff build bullet by
calc call cam camera can cancel cast cd center cfg change check child clear client clone close code
collect color command comp component concat condition config console const continue control convert
cooldown copy cos count create cross cur current custom cut
data db dead debug default delay delete destroy dict dir disable display dist distance divide do dot
down draw dsl dst dt duration
each effect else emit enable end entity era event exec execute exists exp expression
face fade fail false far file filter find fire first float floor follow for force foreach format
forward from func function
game general get gfx global go gohome group
hash head hide highlight hit home hp hud
id if impact in include index info init instance int integer int32 into is item
key kill
last leader left len length level list load local lock log look loop low
main make map math max member message min minus mod move msg mul multiply
name nav near nearest new next no not now npc null number
object off offset on once only open or other out over
param params parent path pause play player plus pos position pow pre print process prompt publish
queue
radius random rand range rate read rect ref remove repeat replace reset result return right root
rot rotate round run
scale scene scp script select self send server set show shutdown sin skill sleep source speed sphere
sqrt start state stop store str string sub sum sync system
table tag take target team terminate test texture this tick time timer to toggle tool trace track
transform tree trigger triger true type
ui unit unlock up update use user util
value var vector vec velocity vel view vfx
wait warn way when while with world write
x y z zero
""".split())
# obj/id/pos/... abbreviations are already in the list. Add project-specific ones.
WORDS |= set("""
acos adjust aggregate alloc app approximately asin asset atan attr await
bind born bounding box buffs byte
camp ceiling chain char charge clamp clean clear completed curve
dependency detect duration
equipment extension
finish fixed
guid
height
immediately internal
md5 memory
num
path
recycle room
section stack story
tasks
uid unimport until update
viewer
yaml yaw
""".split())
WORDS |= {'obj', 'id', 'idx', 'lerp', 'lvl', 'mgr', 'src', 'str', 'sync', 'xp', 'fov', 'gui', 'db'}
MAXW = max(len(w) for w in WORDS)

# These registered DSL names are also C# keywords / type aliases. They are a
# single word (or abbreviation) so snake_case must leave them alone; renaming
# them would rewrite C# syntax such as typeof(byte) and uint across both repos.
KEEP_AS_IS = {'orderby', 'sbyte', 'typeof', 'uint', 'ulong', 'ushort'}
UNDO = {'order_by': 'orderby', 's_byte': 'sbyte', 'type_of': 'typeof',
        'u_int': 'uint', 'u_long': 'ulong', 'u_short': 'ushort'}

# Explicitly reviewed names that the class-name / dictionary path could not
# resolve with certainty. Single words and abbreviations stay as they are;
# compounds are split per word, digits are separate tokens.
OVERRIDES = {
    # --- single words / abbreviations: unchanged ---
    'average': 'average', 'distinct': 'distinct', 'expand': 'expand',
    'eular': 'eular', 'eval': 'eval', 'full': 'full', 'import': 'import',
    'mono': 'mono', 'namespace': 'namespace', 'native': 'native',
    'printf': 'printf', 'pwd': 'pwd', 'quality': 'quality',
    'quaternion': 'quaternion', 'reduce': 'reduce', 'rotation': 'rotation',
    'skip': 'skip', 'suspend': 'suspend', 'top': 'top', 'total': 'total',
    'unused': 'unused', 'where': 'where', 'sys': 'sys', 'pid': 'pid',
    'plist': 'plist', 'envs': 'envs', 'vsync': 'vsync',
    # --- compounds ---
    'assetpath2guid': 'asset_path_2_guid',
    'guid2assetpath': 'guid_2_asset_path',
    'clearequipments': 'clear_equipments',
    'clearnamespacedmessage': 'clear_namespaced_message',
    'clearskills': 'clear_skills',
    'clickonptr': 'click_on_ptr', 'toggleonptr': 'toggle_on_ptr',
    'cmdline': 'cmd_line', 'cmdlineargs': 'cmd_line_args',
    'countdyingnpc': 'count_dying_npc',
    'direxist': 'dir_exist', 'fileexist': 'file_exist', 'pathexist': 'path_exist',
    'elapsedtimeus': 'elapsed_time_us',
    'enablecalculatorlog': 'enable_calculator_log',
    'evalscp': 'eval_scp',
    'findallobjids': 'find_all_obj_ids', 'findobjids': 'find_obj_ids',
    'findbutton': 'find_button', 'finddropdown': 'find_dropdown',
    'findinput': 'find_input', 'findrawimg': 'find_raw_img',
    'findslider': 'find_slider', 'finduiimg': 'find_ui_img',
    'fromjson': 'from_json',
    'getactivityclass': 'get_activity_class',
    'getandroidsdk': 'get_android_sdk',
    'getenv': 'get_env', 'setenv': 'set_env',
    'getguidandfileid': 'get_guid_and_file_id',
    'getmat': 'get_mat', 'setmat': 'set_mat', 'setmats': 'set_mats',
    'getsmat': 'get_smat', 'setsmat': 'set_smat', 'setsmats': 'set_smats',
    'getoriginalprefabroot': 'get_original_prefab_root',
    'getptrcomps': 'get_ptr_comps', 'getptruis': 'get_ptr_uis',
    'getshell': 'get_shell', 'getshelltimeout': 'get_shell_timeout',
    'getstoryvariable': 'get_story_variable',
    'setstoryvariable': 'set_story_variable',
    'gettexturestorage': 'get_texture_storage',
    'isdev': 'is_dev', 'isstoryskipped': 'is_story_skipped',
    'setstoryskipped': 'set_story_skipped',
    'istexsupported': 'is_tex_supported',
    'listalldirs': 'list_all_dirs', 'listdirs': 'list_dirs',
    'listcomponents': 'list_components',
    'localconcurrentmessage': 'local_concurrent_message',
    'localconcurrentnamespacedmessage': 'local_concurrent_namespaced_message',
    'localnamespacedmessage': 'local_namespaced_message',
    'logcomps': 'log_comps', 'logobjs': 'log_objs',
    'matgetcolor': 'mat_get_color', 'matsetcolor': 'mat_set_color',
    'matgetfloat': 'mat_get_float', 'matsetfloat': 'mat_set_float',
    'matgetint': 'mat_get_int', 'matsetint': 'mat_set_int',
    'matgetinteger': 'mat_get_integer', 'matsetinteger': 'mat_set_integer',
    'matgetvector': 'mat_get_vector', 'matsetvector': 'mat_set_vector',
    'orderbydesc': 'order_by_desc',
    'prefdel': 'pref_del', 'prefdelall': 'pref_del_all',
    'prefgetbyjava': 'pref_get_by_java', 'prefsetbyjava': 'pref_set_by_java',
    'prefgetfloat': 'pref_get_float', 'prefsetfloat': 'pref_set_float',
    'prefgetint': 'pref_get_int', 'prefsetint': 'pref_set_int',
    'prefgetstr': 'pref_get_str', 'prefsetstr': 'pref_set_str',
    'raycastcomps': 'raycast_comps', 'raycastuis': 'raycast_uis',
    'realtime': 'real_time', 'realtimewait': 'real_time_wait',
    'storyrealtimewait': 'story_real_time_wait',
    'resourcesystem': 'resource_system',
    'resumelocalmessagehandler': 'resume_local_message_handler',
    'resumelocalnamespacedmessagehandler': 'resume_local_namespaced_message_handler',
    'rndfromlist': 'rnd_from_list',
    'rndvector2': 'rnd_vector_2', 'rndvector3': 'rnd_vector_3',
    'searchcomps': 'search_comps', 'searchobjs': 'search_objs',
    'sendaiconcurrentmessage': 'send_ai_concurrent_message',
    'sendaiconcurrentnamespacedmessage': 'send_ai_concurrent_namespaced_message',
    'sendainamespacedmessage': 'send_ai_namespaced_message',
    'sendconcurrentstorymessage': 'send_concurrent_story_message',
    'setmaxenergy': 'set_max_energy',
    'setnavmeshagentenable': 'set_nav_mesh_agent_enable',
    'storylocalconcurrentmessage': 'story_local_concurrent_message',
    'storylocalconcurrentnamespacedmessage': 'story_local_concurrent_namespaced_message',
    'storylocalnamespacedmessage': 'story_local_namespaced_message',
    'storywaitlocalmessagehandler': 'story_wait_local_message_handler',
    'storywaitlocalnamespacedmessage': 'story_wait_local_namespaced_message',
    'storywaitlocalnamespacedmessagehandler': 'story_wait_local_namespaced_message_handler',
    'supportsgf': 'support_sgf', 'supportsva': 'support_sva',
    'suspendlocalmessagehandler': 'suspend_local_message_handler',
    'suspendlocalnamespacedmessagehandler': 'suspend_local_namespaced_message_handler',
    'sysinfo': 'sys_info',
    'waitlocalmessagehandler': 'wait_local_message_handler',
    'waitlocalnamespacedmessage': 'wait_local_namespaced_message',
    'waitlocalnamespacedmessagehandler': 'wait_local_namespaced_message_handler',
    'wetestheight': 'we_test_height', 'wetestwidth': 'we_test_width',
    'wetestx': 'we_test_x', 'wetesty': 'we_test_y',
}


def walk(root):
    for base, dirs, files in os.walk(root):
        dirs[:] = [d for d in dirs if d not in SKIP_DIRS]
        for fn in files:
            yield os.path.join(base, fn)


def read_text(path):
    with open(path, 'rb') as f:
        b = f.read()
    if b'\x00' in b[:8192]:
        return None, None
    bom = b.startswith(b'\xef\xbb\xbf')
    raw = b[3:] if bom else b
    for enc in ('utf-8', 'gbk'):
        try:
            return raw.decode(enc), (enc, bom)
        except UnicodeDecodeError:
            pass
    return None, None


def write_text(path, text, meta):
    enc, bom = meta
    out = text.encode(enc)
    if bom:
        out = b'\xef\xbb\xbf' + out
    with open(path, 'wb') as f:
        f.write(out)


def split_words(name):
    """Tokenize a lowercase run. Digits are independent tokens.

    Unknown letter runs are kept WHOLE as one token instead of being chopped
    into wrong short words (e.g. 'asset' must not become 'as'+'set'). Unknown
    tokens are reported by scan for human review.
    """
    tokens = []
    unknown = []
    for part in re.split(r'([0-9]+)', name):
        if not part:
            continue
        if part.isdigit():
            tokens.append(part)
            continue
        i = 0
        while i < len(part):
            best = None
            for L in range(min(MAXW, len(part) - i), 0, -1):
                w = part[i:i + L]
                if w in WORDS:
                    best = w
                    break
            if best:
                tokens.append(best)
                i += len(best)
                continue
            # No known word starts here: consume until the next position where
            # a known word does start, keeping this run as a single token.
            j = i + 1
            while j < len(part):
                hit = False
                for L in range(min(MAXW, len(part) - j), 0, -1):
                    if part[j:j + L] in WORDS:
                        hit = True
                        break
                if hit:
                    break
                j += 1
            token = part[i:j]
            tokens.append(token)
            unknown.append(token)
            i = j
    return tokens, unknown


FACTORY = re.compile(r'<\s*([\w.]+)\s*>')
CLASS_SUFFIX = re.compile(r'(Function|Command|Trigger|Triger|Factory|Helper|Api|Exp|Value)$')


def normalize_digits(tokens):
    """A digit is an independent word: 'id2' -> ['id','2']."""
    out = []
    for t in tokens:
        m = re.fullmatch(r'([a-z]+)([0-9]+)', t)
        if m:
            out.append(m.group(1))
            out.append(m.group(2))
        else:
            out.append(t)
    return out


def class_words(cls):
    core = CLASS_SUFFIX.sub('', cls)
    return normalize_digits(
        [w.lower() for w in re.findall(r'[A-Z][a-z0-9]*|[a-z0-9]+', core)])


def build_map():
    names = collections.defaultdict(set)     # name -> repos
    classes = collections.defaultdict(set)   # name -> impl class names
    files = collections.defaultdict(int)
    for key, root in ROOTS.items():
        for path in walk(root):
            if os.path.splitext(path)[1].lower() != '.cs':
                continue
            text, meta = read_text(path)
            if text is None:
                continue
            hit = False
            for line in text.splitlines():
                m = REG.search(line) or TRIGGER.search(line)
                if not m:
                    continue
                hit = True
                names[m.group(1)].add(key)
                fm = FACTORY.search(line)
                if fm:
                    classes[m.group(1)].add(fm.group(1).split('.')[-1])
            if hit:
                files[path] += 1

    mapping = {}
    review = []
    source = collections.Counter()
    skipped = 0
    for old in sorted(names):
        # Scope: all-lowercase concatenated names. CamelCase Register(...)
        # arguments are implClass strings; snake_case names are already done.
        if not re.fullmatch(r'[a-z0-9]+', old) or old in KEEP_AS_IS:
            skipped += 1
            continue
        if old in OVERRIDES:
            source['override'] += 1
            if OVERRIDES[old] != old:
                mapping[old] = OVERRIDES[old]
            continue
        tokens = None
        # Authoritative source: the factory class name carries the word breaks.
        for cls in classes.get(old, ()):
            cand = class_words(cls)
            if cand and ''.join(cand) == old:
                tokens = cand
                break
        if tokens is None:
            tokens, unk = split_words(old)
            if unk:
                # Never guess word boundaries. A wrong split silently breaks
                # every DSL call site, so leave this name for explicit review.
                source['needs_review'] += 1
                review.append(old)
                continue
            tokens = normalize_digits(tokens)
            source['dictionary_clean'] += 1
        else:
            source['class'] += 1
        new = '_'.join(tokens)
        if new != old:
            mapping[old] = new
    return names, mapping, review, files, skipped, source


def cmd_scan():
    names, mapping, review, files, skipped, source = build_map()
    os.makedirs(os.path.dirname(OUT), exist_ok=True)
    with open(OUT, 'w', encoding='utf-8') as f:
        f.write('# old\tnew\trepos\n')
        for old in sorted(mapping):
            f.write('%s\t%s\t%s\n' % (old, mapping[old], ','.join(sorted(names[old]))))
    rev_out = os.path.join(os.path.dirname(OUT), 'rename_review.tsv')
    with open(rev_out, 'w', encoding='utf-8') as f:
        for old in review:
            f.write(old + '\n')
    print('registration files=%d unique names=%d to_rename=%d skipped=%d'
          % (len(files), len(names), len(mapping), skipped))
    for key in ROOTS:
        n = sum(1 for v in names.values() if key in v)
        print('  %-6s names=%d' % (key, n))
    print('token source: %s' % dict(source))
    print('needs explicit review=%d -> %s' % (len(review), rev_out))
    print('map -> ' + OUT)
    return 0


def load_map():
    if not os.path.exists(OUT):
        raise SystemExit('run scan first')
    m = {}
    with open(OUT, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = line.rstrip('\n').split('\t')
            if len(p) >= 2:
                m[p[0]] = p[1]
    return m


def cmd_apply():
    m = load_map()
    pat = re.compile(r'\b(' + '|'.join(
        re.escape(k) for k in sorted(m, key=len, reverse=True)) + r')\b')
    total_files = total_hits = 0
    per_repo = collections.Counter()
    for key, root in ROOTS.items():
        for path in walk(root):
            if os.path.splitext(path)[1].lower() not in TEXT_EXT or is_self(path):
                continue
            text, meta = read_text(path)
            if text is None:
                continue
            new, n = pat.subn(lambda mo: m[mo.group(1)], text)
            if n:
                write_text(path, new, meta)
                total_files += 1
                total_hits += n
                per_repo[key] += n
    print('APPLY files=%d replacements=%d %s' % (total_files, total_hits, dict(per_repo)))


def cmd_undo():
    """Repair C# keywords that a previous blanket replace had rewritten."""
    pat = re.compile(r'\b(' + '|'.join(
        re.escape(k) for k in sorted(UNDO, key=len, reverse=True)) + r')\b')
    files = hits = 0
    for key, root in ROOTS.items():
        for path in walk(root):
            if os.path.splitext(path)[1].lower() not in TEXT_EXT or is_self(path):
                continue
            text, meta = read_text(path)
            if text is None:
                continue
            new, n = pat.subn(lambda mo: UNDO[mo.group(1)], text)
            if n:
                write_text(path, new, meta)
                files += 1
                hits += n
    print('UNDO_KEYWORDS files=%d replacements=%d' % (files, hits))


def cmd_verify():
    m = load_map()
    # An empty pattern would match every word boundary and produce fake hits.
    old_pat = (re.compile(r'\b(' + '|'.join(
        re.escape(k) for k in sorted(m, key=len, reverse=True)) + r')\b')
        if m else None)
    kw_damage = re.compile(r'\b(?:type_of|s_byte|u_short|u_int|u_long)\b')
    leftovers = []
    damaged = []
    for key, root in ROOTS.items():
        for path in walk(root):
            ext = os.path.splitext(path)[1].lower()
            if ext not in TEXT_EXT or is_self(path):
                continue
            text, meta = read_text(path)
            if text is None:
                continue
            if old_pat is not None:
                hits = old_pat.findall(text)
                if hits:
                    leftovers.append((path.replace(root, ''), len(hits), sorted(set(hits))[:4]))
            if ext == '.cs' and kw_damage.search(text):
                damaged.append(path.replace(root, ''))
    print('VERIFY map_entries=%d old-name leftovers=%d files, C# keyword damage=%d files'
          % (len(m), len(leftovers), len(damaged)))
    for p, n, s in leftovers[:20]:
        print('  OLD %-66s x%-4d %s' % (p, n, ','.join(s)))
    for p in damaged[:20]:
        print('  KW  ' + p)
    return 1 if leftovers or damaged else 0


if __name__ == '__main__':
    cmd = sys.argv[1] if len(sys.argv) > 1 else 'scan'
    if cmd == 'scan':
        sys.exit(cmd_scan())
    elif cmd == 'apply':
        sys.exit(cmd_apply())
    elif cmd == 'undo':
        sys.exit(cmd_undo())
    elif cmd == 'verify':
        sys.exit(cmd_verify())
    else:
        raise SystemExit('unknown command: ' + cmd)
