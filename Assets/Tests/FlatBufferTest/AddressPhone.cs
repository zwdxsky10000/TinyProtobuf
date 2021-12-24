// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace FlatBufferTest
{

using global::System;
using global::System.Collections.Generic;
using global::FlatBuffers;

public struct AddressPhone : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_1_12_0(); }
  public static AddressPhone GetRootAsAddressPhone(ByteBuffer _bb) { return GetRootAsAddressPhone(_bb, new AddressPhone()); }
  public static AddressPhone GetRootAsAddressPhone(ByteBuffer _bb, AddressPhone obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public AddressPhone __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Phone { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetPhoneBytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetPhoneBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetPhoneArray() { return __p.__vector_as_array<byte>(6); }
  public string Email { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetEmailBytes() { return __p.__vector_as_span<byte>(8, 1); }
#else
  public ArraySegment<byte>? GetEmailBytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetEmailArray() { return __p.__vector_as_array<byte>(8); }
  public double Serial { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetDouble(o + __p.bb_pos) : (double)0.0; } }
  public float Cost { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<FlatBufferTest.AddressPhone> CreateAddressPhone(FlatBufferBuilder builder,
      int id = 0,
      StringOffset phoneOffset = default(StringOffset),
      StringOffset emailOffset = default(StringOffset),
      double serial = 0.0,
      float cost = 0.0f) {
    builder.StartTable(5);
    AddressPhone.AddSerial(builder, serial);
    AddressPhone.AddCost(builder, cost);
    AddressPhone.AddEmail(builder, emailOffset);
    AddressPhone.AddPhone(builder, phoneOffset);
    AddressPhone.AddId(builder, id);
    return AddressPhone.EndAddressPhone(builder);
  }

  public static void StartAddressPhone(FlatBufferBuilder builder) { builder.StartTable(5); }
  public static void AddId(FlatBufferBuilder builder, int id) { builder.AddInt(0, id, 0); }
  public static void AddPhone(FlatBufferBuilder builder, StringOffset phoneOffset) { builder.AddOffset(1, phoneOffset.Value, 0); }
  public static void AddEmail(FlatBufferBuilder builder, StringOffset emailOffset) { builder.AddOffset(2, emailOffset.Value, 0); }
  public static void AddSerial(FlatBufferBuilder builder, double serial) { builder.AddDouble(3, serial, 0.0); }
  public static void AddCost(FlatBufferBuilder builder, float cost) { builder.AddFloat(4, cost, 0.0f); }
  public static Offset<FlatBufferTest.AddressPhone> EndAddressPhone(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<FlatBufferTest.AddressPhone>(o);
  }
};


}
