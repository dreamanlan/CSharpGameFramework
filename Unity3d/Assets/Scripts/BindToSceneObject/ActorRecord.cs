using UnityEngine;
using System.Collections;

public class ActorRecord : MonoBehaviour
{
    public int id = 1;
    public new string name = string.Empty;
    [HideInInspector]
    public int icon = 1;
    [HideInInspector]
    public int bigIcon = 1;
    [HideInInspector]
    public int type = 2;
    [HideInInspector]
    public string avatar = string.Empty;
    public int skill0 = 0;
    public int skill1 = 0;
    public int skill2 = 0;
    public int skill3 = 0;
    public int skill4 = 0;
    public int skill5 = 0;
    public int skill6 = 0;
    public int skill7 = 0;
    public int skill8 = 0;
    public int bornskill = 1;
    public int deadskill = 2;
    public float size = 1.0f;
    public float logicsize = 1.0f;
    public float speed = 10;
    public float viewrange = 20;
    public float gohomerange = 20;
    [HideInInspector]
    public string avatarFilter = string.Empty;

    public void Reset()
    {
        id = 1;
        name = string.Empty;
        icon = 1;
        bigIcon = 1;
        type = 2;
        avatar = string.Empty;
        skill0 = 0;
        skill1 = 0;
        skill2 = 0;
        skill3 = 0;
        skill4 = 0;
        skill5 = 0;
        skill6 = 0;
        skill7 = 0;
        skill8 = 0;
        bornskill = 1;
        deadskill = 2;
        size = 1.0f;
        logicsize = 1.0f;
        speed = 10;
        viewrange = 20;
        gohomerange = 20;
        avatarFilter = string.Empty;
    }
    public void CopyFrom(TableConfig.Actor actor)
    {
        id = actor.id;
        name = actor.name;
        icon = actor.icon;
        bigIcon = actor.bigIcon;
        type = actor.type;
        avatar = actor.avatar;
        skill0 = actor.skill0;
        skill1 = actor.skill1;
        skill2 = actor.skill2;
        skill3 = actor.skill3;
        skill4 = actor.skill4;
        skill5 = actor.skill5;
        skill6 = actor.skill6;
        skill7 = actor.skill7;
        skill8 = actor.skill8;
        bornskill = actor.bornskill;
        deadskill = actor.deadskill;
        size = actor.size;
        logicsize = actor.logicsize;
        speed = actor.speed;
        viewrange = actor.viewrange;
        gohomerange = actor.gohomerange;
    }
    public void CopyTo(TableConfig.Actor actor)
    {
        actor.id = id;
        actor.name = name;
        actor.icon = icon;
        actor.bigIcon = bigIcon;
        actor.type = type;
        actor.avatar = avatar;
        actor.skill0 = skill0;
        actor.skill1 = skill1;
        actor.skill2 = skill2;
        actor.skill3 = skill3;
        actor.skill4 = skill4;
        actor.skill5 = skill5;
        actor.skill6 = skill6;
        actor.skill7 = skill7;
        actor.skill8 = skill8;
        actor.bornskill = bornskill;
        actor.deadskill = deadskill;
        actor.size = size;
        actor.logicsize = logicsize;
        actor.speed = speed;
        actor.viewrange = viewrange;
        actor.gohomerange = gohomerange;
    }
    public string GetClipboardText()
    {
        return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}", id, name, icon, bigIcon, type, avatar, skill0, skill1, skill2, skill3, skill4, skill5, skill6, skill7, skill8, bornskill, deadskill, size, logicsize, speed, viewrange, gohomerange);
    }
}
