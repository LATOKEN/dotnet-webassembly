using System.IO;
using System.Text;
using WebAssembly.Runtime.Compilation;

namespace WebAssembly.Runtime
{
    static class ClangDebugSymbolsParser
    {
        internal static string?[]? ParseDebugNames(
            Reader reader, uint payloadLength, long preNameOffset, Signature[]? functionSignatures
        )
        {
            var debugNames = new string?[1000];
            var customName = reader.ReadString(reader.ReadVarUInt32()); //Name
            var bytes = reader.ReadBytes(payloadLength - checked((uint) (reader.Offset - preNameOffset))); //Content
            var customReader = new BinaryReader(new MemoryStream(bytes));

            switch (customName)
            {
                case "dylink":
                    /* TODO: "not implemented" */
                    return null;
                case "name":
                {
                    var type = ReadULEB128(customReader);
                    ReadULEB128(customReader);
                    // enum : unsigned { WASM_NAMES_FUNCTION = 0x1, WASM_NAMES_LOCAL = 0x2 }
                    if (type != 0x1)
                        break;
                    var count = (int) ReadULEB128(customReader);
                    if (count > functionSignatures.Length)
                        break;

                    //Console.WriteLine("Functions:\n----------------------------");
                    while (count-- > 0)
                    {
                        var index = ReadULEB128(customReader);
                        var len = ReadULEB128(customReader);
                        var name = customReader.ReadBytes((int) len);
                        var parsedName = Encoding.ASCII.GetString(name);
                        //Console.WriteLine(" " + index + " : " + parsedName);
                        if (debugNames[index] != null)
                            continue;
                        debugNames[index] = parsedName;
                    }

                    //Console.WriteLine("----------------------------");
                }
                    break;
                case "linking":
                    /* TODO: "not implemented" */
                    return null;
                case "producers":
                    /* TODO: "not implemented" */
                    return null;
                case "reloc.":
                    /* TODO: "not implemented" */
                    return null;
            }

            return debugNames;
        }

        private static ulong ReadULEB128(BinaryReader binaryReader)
        {
            ulong result = 0;
            int shift = 0;
            while (true)
            {
                byte b = binaryReader.ReadByte();
                result |= ((ulong) (b & 0x7f)) << shift;
                if ((b & 0x80) != 0x80)
                    break;
                shift += 7;
            }

            return result;
        }
    }
}