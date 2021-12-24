/**************************************************************************************************
                                  自动生成的Protobuf代码  请勿手动修改
**************************************************************************************************/

using Protobuf;

namespace tinyprotobuf
{
	public partial class Person : IMessage
	{

		private int _id = default(int);

		public int id
		{
			get
			{
				return _id;
			}

			set
			{
				_id = value;
			}
		}

		private string _name = default(string);

		public string name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		private int _age = default(int);

		public int age
		{
			get
			{
				return _age;
			}

			set
			{
				_age = value;
			}
		}

		private System.Collections.Generic.List<int> _nums = new System.Collections.Generic.List<int>();

		public System.Collections.Generic.List<int> nums
		{
			get
			{
				return _nums;
			}

			set
			{
				_nums = value;
			}
		}

		private System.Collections.Generic.List<float> _mons = new System.Collections.Generic.List<float>();

		public System.Collections.Generic.List<float> mons
		{
			get
			{
				return _mons;
			}

			set
			{
				_mons = value;
			}
		}

		private System.Collections.Generic.List<double> _years = new System.Collections.Generic.List<double>();

		public System.Collections.Generic.List<double> years
		{
			get
			{
				return _years;
			}

			set
			{
				_years = value;
			}
		}

		private AddressBook _book = default(AddressBook);

		public AddressBook book
		{
			get
			{
				return _book;
			}

			set
			{
				_book = value;
			}
		}

		public void Encode(Protobuf.CodedOutputStream outStream)
		{
			if (id != default(int))
			{
				outStream.WriteTag(8);
				outStream.WriteInt32(id);
			}

			if (name != default(string))
			{
				outStream.WriteTag(18);
				outStream.WriteString(name);
			}

			if (age != default(int))
			{
				outStream.WriteTag(24);
				outStream.WriteInt32(age);
			}

			if ( nums != null && nums.Count >0 )
			{
				outStream.WriteTag(34);
				outStream.BeginLengthRecord();
				for (int i = 0,len = nums.Count; i < len; ++i)
				{
					outStream.WriteInt32(nums[i]);
				}
				outStream.EndLengthRecord();
			}

			if ( mons != null && mons.Count >0 )
			{
				outStream.WriteTag(42);
				outStream.BeginLengthRecord();
				for (int i = 0,len = mons.Count; i < len; ++i)
				{
					outStream.WriteFloat(mons[i]);
				}
				outStream.EndLengthRecord();
			}

			if ( years != null && years.Count >0 )
			{
				outStream.WriteTag(50);
				outStream.BeginLengthRecord();
				for (int i = 0,len = years.Count; i < len; ++i)
				{
					outStream.WriteDouble(years[i]);
				}
				outStream.EndLengthRecord();
			}

			if (book != null)
			{
				outStream.WriteTag(58);
				outStream.BeginLengthRecord();
				book.Encode(outStream);
				outStream.EndLengthRecord();
			}

		}

		public void Decode(Protobuf.CodedInputStream inStream)
		{
			uint tag;

			while ((tag = inStream.ReadTag()) != 0)
			{
				int fieldNum = Protobuf.WireFormat.GetTagFieldNumber(tag);

				switch (fieldNum)
				{
					case 1:
					{
						id = inStream.ReadInt32();
					}
					break;
					case 2:
					{
						name = inStream.ReadString();
					}
					break;
					case 3:
					{
						age = inStream.ReadInt32();
					}
					break;
					case 4:
					{
						if (nums == null){ nums = new System.Collections.Generic.List<int>(); }

						if (Protobuf.WireFormat.IsPacked(tag))
						{
							int len = inStream.ReadLength();
							if(len > 0)
							{
								int oldLimit = inStream.PushLimit(len);
								while(!inStream.ReachedLimit) { nums.Add(inStream.ReadInt32()); }

								inStream.PopLimit(oldLimit);
							}
							else
							{
								do
								{
									nums.Add(inStream.ReadInt32());
								}
								while (inStream.MaybeConsumeTag(tag));
							}
						}
					}
					break;
					case 5:
					{
						if (mons == null){ mons = new System.Collections.Generic.List<float>(); }

						if (Protobuf.WireFormat.IsPacked(tag))
						{
							int len = inStream.ReadLength();
							if(len > 0)
							{
								int oldLimit = inStream.PushLimit(len);
								while(!inStream.ReachedLimit) { mons.Add(inStream.ReadFloat()); }

								inStream.PopLimit(oldLimit);
							}
							else
							{
								do
								{
									mons.Add(inStream.ReadFloat());
								}
								while (inStream.MaybeConsumeTag(tag));
							}
						}
					}
					break;
					case 6:
					{
						if (years == null){ years = new System.Collections.Generic.List<double>(); }

						if (Protobuf.WireFormat.IsPacked(tag))
						{
							int len = inStream.ReadLength();
							if(len > 0)
							{
								int oldLimit = inStream.PushLimit(len);
								while(!inStream.ReachedLimit) { years.Add(inStream.ReadDouble()); }

								inStream.PopLimit(oldLimit);
							}
							else
							{
								do
								{
									years.Add(inStream.ReadDouble());
								}
								while (inStream.MaybeConsumeTag(tag));
							}
						}
					}
					break;
					case 7:
					{
						if (book == null) { book = new AddressBook(); }

						int len = inStream.ReadLength();
						int oldLimit = inStream.PushLimit(len);
						book.Decode(inStream);

						if (!inStream.ReachedLimit) throw Protobuf.InvalidProtocolBufferException.TruncatedMessage();

						inStream.PopLimit(oldLimit);
					}
					break;
					default:
						inStream.SkipLastField();
					break;
				}
			}
		}

	}

	public partial class AddressBook : IMessage
	{

		private int _id = default(int);

		public int id
		{
			get
			{
				return _id;
			}

			set
			{
				_id = value;
			}
		}

		private string _address = default(string);

		public string address
		{
			get
			{
				return _address;
			}

			set
			{
				_address = value;
			}
		}

		private System.Collections.Generic.List<AddressPhone> _phones = new System.Collections.Generic.List<AddressPhone>();

		public System.Collections.Generic.List<AddressPhone> phones
		{
			get
			{
				return _phones;
			}

			set
			{
				_phones = value;
			}
		}

		public void Encode(Protobuf.CodedOutputStream outStream)
		{
			if (id != default(int))
			{
				outStream.WriteTag(8);
				outStream.WriteInt32(id);
			}

			if (address != default(string))
			{
				outStream.WriteTag(18);
				outStream.WriteString(address);
			}

			if ( phones != null && phones.Count >0 )
			{
				for (int i = 0,len = phones.Count; i < len; ++i)
				{
					outStream.WriteTag(26);
					outStream.BeginLengthRecord();
					phones[i].Encode(outStream);
					outStream.EndLengthRecord();
				}
			}

		}

		public void Decode(Protobuf.CodedInputStream inStream)
		{
			uint tag;

			while ((tag = inStream.ReadTag()) != 0)
			{
				int fieldNum = Protobuf.WireFormat.GetTagFieldNumber(tag);

				switch (fieldNum)
				{
					case 1:
					{
						id = inStream.ReadInt32();
					}
					break;
					case 2:
					{
						address = inStream.ReadString();
					}
					break;
					case 3:
					{
						if (phones == null){ phones = new System.Collections.Generic.List<AddressPhone>(); }

						do
						{
							int len = inStream.ReadLength();
							int oldLimit = inStream.PushLimit(len);
							var obj = new AddressPhone();
							obj.Decode(inStream);
							inStream.CheckReadEndOfStreamTag();

							if (!inStream.ReachedLimit) throw Protobuf.InvalidProtocolBufferException.TruncatedMessage();

							phones.Add(obj);
							inStream.PopLimit(oldLimit);
						}
						while(inStream.MaybeConsumeTag(tag));
					}
					break;
					default:
						inStream.SkipLastField();
					break;
				}
			}
		}

	}

	public partial class AddressPhone : IMessage
	{

		private int _id = default(int);

		public int id
		{
			get
			{
				return _id;
			}

			set
			{
				_id = value;
			}
		}

		private string _phone = default(string);

		public string phone
		{
			get
			{
				return _phone;
			}

			set
			{
				_phone = value;
			}
		}

		private string _email = default(string);

		public string email
		{
			get
			{
				return _email;
			}

			set
			{
				_email = value;
			}
		}

		private double _serial = default(double);

		public double serial
		{
			get
			{
				return _serial;
			}

			set
			{
				_serial = value;
			}
		}

		private float _cost = default(float);

		public float cost
		{
			get
			{
				return _cost;
			}

			set
			{
				_cost = value;
			}
		}

		public void Encode(Protobuf.CodedOutputStream outStream)
		{
			if (id != default(int))
			{
				outStream.WriteTag(8);
				outStream.WriteInt32(id);
			}

			if (phone != default(string))
			{
				outStream.WriteTag(18);
				outStream.WriteString(phone);
			}

			if (email != default(string))
			{
				outStream.WriteTag(26);
				outStream.WriteString(email);
			}

			if (serial != default(double))
			{
				outStream.WriteTag(33);
				outStream.WriteDouble(serial);
			}

			if (cost != default(float))
			{
				outStream.WriteTag(45);
				outStream.WriteFloat(cost);
			}

		}

		public void Decode(Protobuf.CodedInputStream inStream)
		{
			uint tag;

			while ((tag = inStream.ReadTag()) != 0)
			{
				int fieldNum = Protobuf.WireFormat.GetTagFieldNumber(tag);

				switch (fieldNum)
				{
					case 1:
					{
						id = inStream.ReadInt32();
					}
					break;
					case 2:
					{
						phone = inStream.ReadString();
					}
					break;
					case 3:
					{
						email = inStream.ReadString();
					}
					break;
					case 4:
					{
						serial = inStream.ReadDouble();
					}
					break;
					case 5:
					{
						cost = inStream.ReadFloat();
					}
					break;
					default:
						inStream.SkipLastField();
					break;
				}
			}
		}

	}

}
