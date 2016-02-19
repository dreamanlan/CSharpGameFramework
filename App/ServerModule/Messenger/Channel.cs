using SharpSvrAPI;

enum Channel : byte
{
  A = CoreMessage.Type.kExtendBegin,
  B,
  C,
  Count,
}