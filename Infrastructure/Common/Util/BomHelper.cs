using System;
using System.IO;
using System.Text;

namespace ScriptableFramework
{
    /// <summary>
    /// UTF-8 BOM detection and BOM-preserving encoding helpers.
    /// Used by file modification APIs to keep the original file's BOM state on overwrite.
    /// </summary>
    public static class BomHelper
    {
        /// <summary>
        /// Returns true if the file at fullPath starts with UTF-8 BOM (EF BB BF).
        /// Returns false if file is shorter than 3 bytes, missing BOM, or not accessible.
        /// </summary>
        public static bool HasUtf8Bom(string fullPath)
        {
            try {
                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    if (fs.Length < 3)
                        return false;
                    byte[] head = new byte[3];
                    int read = fs.Read(head, 0, 3);
                    return read == 3 && head[0] == 0xEF && head[1] == 0xBB && head[2] == 0xBF;
                }
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Returns an Encoding that matches the existing file's encoding and BOM state.
        /// Detection order: BOM-based (UTF-32/UTF-16/UTF-8) first, then UTF-8 validity check.
        /// If the file does not exist, or the encoding cannot be determined, falls back to
        /// fallbackEncoding without BOM when supplied; otherwise UTF-8 with BOM controlled
        /// by defaultBom.
        /// </summary>
        public static Encoding GetEncodingPreservingBom(string fullPath, bool defaultBom = true, Encoding fallbackEncoding = null)
        {
            if (!File.Exists(fullPath)) {
                return new UTF8Encoding(defaultBom);
            }

            try {
                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    if (fs.Length == 0) {
                        return new UTF8Encoding(false);
                    }

                    // Read up to 4 bytes for BOM detection.
                    int bomLen = (int)Math.Min(4, fs.Length);
                    byte[] head = new byte[bomLen];
                    int read = fs.Read(head, 0, bomLen);

                    // Detect by BOM (order matters: UTF-32 LE shares FF FE with UTF-16 LE).
                    if (read >= 4 && head[0] == 0x00 && head[1] == 0x00 && head[2] == 0xFE && head[3] == 0xFF) {
                        return new UTF32Encoding(bigEndian: true, byteOrderMark: true);
                    }
                    if (read >= 4 && head[0] == 0xFF && head[1] == 0xFE && head[2] == 0x00 && head[3] == 0x00) {
                        return new UTF32Encoding(bigEndian: false, byteOrderMark: true);
                    }
                    if (read >= 3 && head[0] == 0xEF && head[1] == 0xBB && head[2] == 0xBF) {
                        return new UTF8Encoding(true);
                    }
                    if (read >= 2 && head[0] == 0xFE && head[1] == 0xFF) {
                        return new UnicodeEncoding(bigEndian: true, byteOrderMark: true);
                    }
                    if (read >= 2 && head[0] == 0xFF && head[1] == 0xFE) {
                        return new UnicodeEncoding(bigEndian: false, byteOrderMark: true);
                    }

                    // No BOM: verify UTF-8 validity over the first 64KB.
                    long sampleLen = Math.Min(fs.Length, 64 * 1024);
                    byte[] sample = new byte[sampleLen];
                    Array.Copy(head, 0, sample, 0, read);
                    int extra = fs.Read(sample, read, (int)sampleLen - read);
                    int total = read + extra;

                    var validator = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
                    try {
                        validator.GetString(sample, 0, total);
                        return new UTF8Encoding(false);
                    }
                    catch {
                        return fallbackEncoding != null
                            ? GetEncodingWithoutBom(fallbackEncoding)
                            : new UTF8Encoding(defaultBom);
                    }
                }
            }
            catch {
                return fallbackEncoding != null
                    ? GetEncodingWithoutBom(fallbackEncoding)
                    : new UTF8Encoding(defaultBom);
            }
        }

        private static Encoding GetEncodingWithoutBom(Encoding encoding)
        {
            if (encoding is UTF8Encoding) {
                return new UTF8Encoding(false);
            }
            if (encoding is UnicodeEncoding) {
                bool bigEndian = encoding.CodePage == 1201;
                return new UnicodeEncoding(bigEndian, false);
            }
            if (encoding is UTF32Encoding) {
                bool bigEndian = encoding.CodePage == 12001;
                return new UTF32Encoding(bigEndian, false);
            }
            return encoding;
        }

        /// <summary>
        /// Detect whether the file at fullPath starts with any BOM (UTF-8/UTF-16/UTF-32).
        /// Returns true if any BOM is found, false otherwise.
        /// </summary>
        private static bool HasAnyBom(string fullPath)
        {
            try {
                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    if (fs.Length < 2) return false;
                    int bomLen = (int)Math.Min(4, fs.Length);
                    byte[] head = new byte[bomLen];
                    int read = fs.Read(head, 0, bomLen);
                    if (read >= 4 && head[0] == 0x00 && head[1] == 0x00 && head[2] == 0xFE && head[3] == 0xFF) return true;
                    if (read >= 4 && head[0] == 0xFF && head[1] == 0xFE && head[2] == 0x00 && head[3] == 0x00) return true;
                    if (read >= 3 && head[0] == 0xEF && head[1] == 0xBB && head[2] == 0xBF) return true;
                    if (read >= 2 && head[0] == 0xFE && head[1] == 0xFF) return true;
                    if (read >= 2 && head[0] == 0xFF && head[1] == 0xFE) return true;
                    return false;
                }
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Parse encoding spec from BoxedValue (string name or int codepage), with optional
        /// "-bom"/"-nobom"/"-no-bom" suffix on string names to control BOM emission.
        /// </summary>
        public static Encoding GetEncoding(BoxedValue v, string fullPath)
        {
            return GetEncoding(v, fullPath, preserveExistingFile: false);
        }

        /// <summary>
        /// Parse an encoding for a write operation. Existing files preserve their detected
        /// encoding and BOM state regardless of suffix. For an existing non-UTF-8 file without
        /// BOM, the requested encoding is used as the fallback without adding a BOM. Suffixes
        /// control BOM emission only for new files.
        /// </summary>
        public static Encoding GetEncodingForWrite(BoxedValue v, string fullPath)
        {
            return GetEncoding(v, fullPath, preserveExistingFile: true);
        }

        private static Encoding GetEncoding(BoxedValue v, string fullPath, bool preserveExistingFile)
        {
            string asString = v.AsString;
            bool emitBom = true;
            bool bomExplicit = false;

            if (asString != null) {
                // Match longest suffix first: -no-bom / -nobom before -bom.
                if (asString.EndsWith("-no-bom", StringComparison.OrdinalIgnoreCase)) {
                    emitBom = false;
                    bomExplicit = true;
                    asString = asString.Substring(0, asString.Length - 7);
                }
                else if (asString.EndsWith("-nobom", StringComparison.OrdinalIgnoreCase)) {
                    emitBom = false;
                    bomExplicit = true;
                    asString = asString.Substring(0, asString.Length - 6);
                }
                else if (asString.EndsWith("-bom", StringComparison.OrdinalIgnoreCase)) {
                    emitBom = true;
                    bomExplicit = true;
                    asString = asString.Substring(0, asString.Length - 4);
                }
            }

            Encoding baseEncoding;
            try {
                if (asString != null) {
                    baseEncoding = Encoding.GetEncoding(asString);
                }
                else if (v.IsInteger) {
                    int codepage = v.GetInt();
                    baseEncoding = Encoding.GetEncoding(codepage);
                }
                else {
                    baseEncoding = Encoding.UTF8;
                }
            }
            catch {
                baseEncoding = Encoding.UTF8;
            }

            if (preserveExistingFile && File.Exists(fullPath)) {
                return GetEncodingPreservingBom(fullPath, defaultBom: false, fallbackEncoding: baseEncoding);
            }

            return ApplyBomPolicy(baseEncoding, fullPath, emitBom, bomExplicit);
        }

        /// <summary>
        /// Apply BOM policy to the base encoding based on file existence and explicit flag.
        /// Only BOM-capable encodings (UTF8Encoding, UnicodeEncoding, UTF32Encoding) are adjusted.
        /// </summary>
        private static Encoding ApplyBomPolicy(Encoding baseEncoding, string fullPath, bool emitBom, bool bomExplicit)
        {
            if (baseEncoding is UTF8Encoding utf8) {
                bool currentEmit = utf8.GetPreamble().Length > 0;
                if (currentEmit == emitBom) return utf8;
                if (!bomExplicit && File.Exists(fullPath)) {
                    bool fileHasBom = HasAnyBom(fullPath);
                    emitBom = fileHasBom;
                }
                return new UTF8Encoding(emitBom);
            }
            if (baseEncoding is UnicodeEncoding unicode) {
                bool bigEndian = unicode.GetPreamble().Length >= 2 && unicode.GetPreamble()[0] == 0xFE;
                bool currentEmit = unicode.GetPreamble().Length > 0;
                if (currentEmit == emitBom) return unicode;
                if (!bomExplicit && File.Exists(fullPath)) {
                    emitBom = HasAnyBom(fullPath);
                }
                return new UnicodeEncoding(bigEndian, emitBom);
            }
            if (baseEncoding is UTF32Encoding utf32) {
                bool bigEndian = utf32.GetPreamble().Length >= 4 && utf32.GetPreamble()[0] == 0x00;
                bool currentEmit = utf32.GetPreamble().Length > 0;
                if (currentEmit == emitBom) return utf32;
                if (!bomExplicit && File.Exists(fullPath)) {
                    emitBom = HasAnyBom(fullPath);
                }
                return new UTF32Encoding(bigEndian, emitBom);
            }
            return baseEncoding;
        }
    }
}
