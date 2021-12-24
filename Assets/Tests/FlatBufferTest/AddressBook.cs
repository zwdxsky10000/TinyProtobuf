// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatBufferTest
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct AddressBook : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static AddressBook GetRootAsAddressBook(ByteBuffer _bb) { return GetRootAsAddressBook(_bb, new AddressBook()); }
  public static AddressBook GetRootAsAddressBook(ByteBuffer _bb, AddressBook obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public AddressBook __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Address { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetAddressBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetAddressBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetAddressArray() { return __p.__vector_as_array<byte>(6); }
  public FlatBufferTest.AddressPhone? Phones(int j) { int o = __p.__offset(8); return o != 0 ? (FlatBufferTest.AddressPhone?)(new FlatBufferTest.AddressPhone()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int PhonesLength { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<FlatBufferTest.AddressBook> CreateAddressBook(FlatBufferBuilder builder,
      int id = 0,
      StringOffset addressOffset = default(StringOffset),
      VectorOffset phonesOffset = default(VectorOffset)) {
    builder.StartTable(3);
    AddressBook.AddPhones(builder, phonesOffset);
    AddressBook.AddAddress(builder, addressOffset);
    AddressBook.AddId(builder, id);
    return AddressBook.EndAddressBook(builder);
  }

  public static void StartAddressBook(FlatBufferBuilder builder) { builder.StartTable(3); }
  public static void AddId(FlatBufferBuilder builder, int id) { builder.AddInt(0, id, 0); }
  public static void AddAddress(FlatBufferBuilder builder, StringOffset addressOffset) { builder.AddOffset(1, addressOffset.Value, 0); }
  public static void AddPhones(FlatBufferBuilder builder, VectorOffset phonesOffset) { builder.AddOffset(2, phonesOffset.Value, 0); }
  public static VectorOffset CreatePhonesVector(FlatBufferBuilder builder, Offset<FlatBufferTest.AddressPhone>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreatePhonesVectorBlock(FlatBufferBuilder builder, Offset<FlatBufferTest.AddressPhone>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartPhonesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<FlatBufferTest.AddressBook> EndAddressBook(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatBufferTest.AddressBook>(o);
  }
};


}