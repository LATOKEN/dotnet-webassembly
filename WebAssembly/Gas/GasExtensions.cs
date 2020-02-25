using System.Collections.Generic;

namespace WebAssembly.Gas
{
    /// <summary>
    /// Extensions for gas metering
    /// </summary>
    public static class GasExtensions
    {
        internal class OpCodeMeteringMetadata
        {
            internal uint Gas { get; }
            internal bool IsFlowControl { get; }

            public OpCodeMeteringMetadata(uint gas)
            {
                Gas = gas;
                IsFlowControl = false;
            }

            public OpCodeMeteringMetadata(uint gas, bool isFlowControl)
            {
                Gas = gas;
                IsFlowControl = isFlowControl;
            }
        }

        private static readonly IDictionary<OpCode, OpCodeMeteringMetadata> OpCodeMetadata =
            new Dictionary<OpCode, OpCodeMeteringMetadata>
            {
                {OpCode.Unreachable, new OpCodeMeteringMetadata(1, true)},
                {OpCode.NoOperation, new OpCodeMeteringMetadata(1, false)},
                {OpCode.Block, new OpCodeMeteringMetadata(1, true)},
                {OpCode.Loop, new OpCodeMeteringMetadata(1, true)},
                {OpCode.If, new OpCodeMeteringMetadata(1, true)},
                {OpCode.Else, new OpCodeMeteringMetadata(90, true)},
                {OpCode.End, new OpCodeMeteringMetadata(1, true)},
                {OpCode.Branch, new OpCodeMeteringMetadata(90, true)},
                {OpCode.BranchIf, new OpCodeMeteringMetadata(90, true)},
                {OpCode.BranchTable, new OpCodeMeteringMetadata(120, true)},
                {OpCode.Return, new OpCodeMeteringMetadata(90, true)},
                {OpCode.Call, new OpCodeMeteringMetadata(90, true)},
                {OpCode.CallIndirect, new OpCodeMeteringMetadata(10000, true)},
                {OpCode.Drop, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Select, new OpCodeMeteringMetadata(120, true)},
                {OpCode.LocalGet, new OpCodeMeteringMetadata(120, false)},
                {OpCode.LocalSet, new OpCodeMeteringMetadata(120, false)},
                {OpCode.LocalTee, new OpCodeMeteringMetadata(120, false)},
                {OpCode.GlobalGet, new OpCodeMeteringMetadata(120, false)},
                {OpCode.GlobalSet, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Load, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Float32Load, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Float64Load, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Load8Signed, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Load8Unsigned, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Load16Signed, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Load16Unsigned, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load8Signed, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load8Unsigned, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load16Signed, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load16Unsigned, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load32Signed, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Load32Unsigned, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Store, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Store, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Float32Store, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Float64Store, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Store8, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int32Store16, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Store8, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Store16, new OpCodeMeteringMetadata(120, false)},
                {OpCode.Int64Store32, new OpCodeMeteringMetadata(120, false)},
                {OpCode.MemorySize, new OpCodeMeteringMetadata(100, false)}, // TODO: why it was called CurrentMemory?
                {OpCode.MemoryGrow, new OpCodeMeteringMetadata(10000, false)},
                {OpCode.Int32Constant, new OpCodeMeteringMetadata(1, false)},
                {OpCode.Int64Constant, new OpCodeMeteringMetadata(1, false)},
                {OpCode.Float32Constant, new OpCodeMeteringMetadata(1, false)},
                {OpCode.Float64Constant, new OpCodeMeteringMetadata(1, false)},
                {OpCode.Int32EqualZero, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32Equal, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32NotEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32LessThanSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32LessThanUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32GreaterThanSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32GreaterThanUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32LessThanOrEqualSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32LessThanOrEqualUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32GreaterThanOrEqualSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32GreaterThanOrEqualUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64EqualZero, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64Equal, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64NotEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64LessThanSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64LessThanUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64GreaterThanSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64GreaterThanUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64LessThanOrEqualSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64LessThanOrEqualUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64GreaterThanOrEqualSigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64GreaterThanOrEqualUnsigned, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float32Equal, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float32NotEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float32LessThan, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float32GreaterThan, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float32LessThanOrEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float32GreaterThanOrEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float64Equal, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float64NotEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float64LessThan, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float64GreaterThan, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float64LessThanOrEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Float64GreaterThanOrEqual, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32CountLeadingZeroes, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32CountTrailingZeroes, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32CountOneBits, new OpCodeMeteringMetadata(100, false)},
                {OpCode.Int32Add, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32Subtract, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32Multiply, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32DivideSigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int32DivideUnsigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int32RemainderSigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int32RemainderUnsigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int32And, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32Or, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32ExclusiveOr, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int32ShiftLeft, new OpCodeMeteringMetadata(67, false)},
                {OpCode.Int32ShiftRightSigned, new OpCodeMeteringMetadata(67, false)},
                {OpCode.Int32ShiftRightUnsigned, new OpCodeMeteringMetadata(67, false)},
                {OpCode.Int32RotateLeft, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32RotateRight, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64CountLeadingZeroes, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64CountTrailingZeroes, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64CountOneBits, new OpCodeMeteringMetadata(100, false)},
                {OpCode.Int64Add, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64Subtract, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64Multiply, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64DivideSigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int64DivideUnsigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int64RemainderSigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int64RemainderUnsigned, new OpCodeMeteringMetadata(36000, false)},
                {OpCode.Int64And, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64Or, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64ExclusiveOr, new OpCodeMeteringMetadata(45, false)},
                {OpCode.Int64ShiftLeft, new OpCodeMeteringMetadata(67, false)},
                {OpCode.Int64ShiftRightSigned, new OpCodeMeteringMetadata(67, false)},
                {OpCode.Int64ShiftRightUnsigned, new OpCodeMeteringMetadata(67, false)},
                {OpCode.Int64RotateLeft, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64RotateRight, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Absolute, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Negate, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Ceiling, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Floor, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Truncate, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Nearest, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32SquareRoot, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Add, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Subtract, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Multiply, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Divide, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Minimum, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32Maximum, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32CopySign, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Absolute, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Negate, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Ceiling, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Floor, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Truncate, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Nearest, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64SquareRoot, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Add, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Subtract, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Multiply, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Divide, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Minimum, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64Maximum, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64CopySign, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32WrapInt64, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32TruncateFloat32Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32TruncateFloat32Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32TruncateFloat64Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32TruncateFloat64Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64ExtendInt32Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64ExtendInt32Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64TruncateFloat32Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64TruncateFloat32Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64TruncateFloat64Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64TruncateFloat64Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32ConvertInt32Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32ConvertInt32Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32ConvertInt64Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32ConvertInt64Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32DemoteFloat64, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64ConvertInt32Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64ConvertInt32Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64ConvertInt64Signed, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64ConvertInt64Unsigned, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64PromoteFloat32, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int32ReinterpretFloat32, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Int64ReinterpretFloat64, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float32ReinterpretInt32, new OpCodeMeteringMetadata(90, false)},
                {OpCode.Float64ReinterpretInt64, new OpCodeMeteringMetadata(90, false)},
            };
        
        internal static uint Gas(this Instruction instruction)
        {
            return OpCodeMetadata[instruction.OpCode].Gas;
        }

        internal static bool IsFlowControl(this Instruction instruction)
        {
            return OpCodeMetadata[instruction.OpCode].IsFlowControl;
        }
    }
}