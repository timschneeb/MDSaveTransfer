namespace MDSaveTransfer.Data;

public enum SaveDataFormat
{
    /** Used by PC version */
    Binary,
    /** Used by Switch version */
    SimplifiedJson,
    /** For debugging purposes only. Holds same data as binary format, except in human-readable form */
    TypesafeJson,
}