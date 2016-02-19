using System;

// about to deprecated
internal sealed class DataTypeNotFound : ApplicationException
{
    internal DataTypeNotFound(string job_full_name)
        : base(job_full_name + " is not found in all available assemblies")
    { }
}

internal sealed class EmptyRecord : ApplicationException
{
    internal EmptyRecord(string type_name, string pri_key_val)
        : base(string.Format("record for type({0}) with key({1}) is empty", type_name, pri_key_val))
    { }
}

internal sealed class PrimaryKeyNotFound : ApplicationException
{
    internal PrimaryKeyNotFound(string data, string pri_key_name)
        : base(string.Format("{0}: the value to primary key {1} is not found", data, pri_key_name))
    { }
}

internal sealed class SizeOptionNotSet : ApplicationException
{
    internal SizeOptionNotSet(string field)
        : base(string.Format("SizeOption MUST BE SET on {0}", field))
    { }
}

internal sealed class DataChecksumError : ApplicationException
{
    internal DataChecksumError(int expect, int actual)
        : base(string.Format("Checksum EXPECT({0}), ACTUAL({1})", expect, actual))
    {
    }
}

internal sealed class NullDataMessage : ApplicationException
{
    internal NullDataMessage(uint id)
        : base("null data message id: " + id)
    { }
}