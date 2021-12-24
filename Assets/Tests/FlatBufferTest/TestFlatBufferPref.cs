using System;
using UnityEngine;
using FlatBuffers;
using System.IO;

namespace FlatBufferTest
{
    public class TestFlatBufferPref : MonoBehaviour
    {
        public int loopCount = 1000;
        
        private void Start()
        {
            string output = Application.dataPath + "/../";
            TestPref(output);
        }
        
        void TestPref(string output)
        {
            
            using (var file = File.Create(Path.Combine(output, "person.bin"))) {
                
                UnityEngine.Profiling.Profiler.BeginSample("TestFlatBufferPref-Serialize");
                for(int i = 0; i < loopCount; ++i)
                {
                    var flatBuilder = new FlatBufferBuilder(1024);

                    var nums = new int[] {12, 34, 56};
                    var mons = new float[] {4.3f, 5.4f, 6.5f};
                    var years = new double[] {5, 4, 4, 2};
                    var phonestr = new string[] {"13141231212", "13141231212", "13141231215", "13141231213", "13141231218", "13141231217"};
                    var emailstr = new string[] {"13123@qq.com", "13123@qq.com", "13123323@qq.com", "1312353@qq.com", "1312313@qq.com", "1312363@qq.com"};
                    
                    var phonesOffsets = new Offset<AddressPhone>[6];
                    for (int j = 0; j < 6; ++j)
                    {
                        var phoneOffset = flatBuilder.CreateString(phonestr[j]);
                        var emailOffset = flatBuilder.CreateString(phonestr[j]);
                        var phone = AddressPhone.CreateAddressPhone(flatBuilder, 10000 + j, phoneOffset, emailOffset, 3.1415923,
                            1.9f);
                        phonesOffsets[j] = phone;
                    }
                    
                    var addressOffset = flatBuilder.CreateString("shenzhen,nanshan");
                    var nameOffset = flatBuilder.CreateString("toyqiu");
                    var vectorOffset = AddressBook.CreatePhonesVector(flatBuilder, phonesOffsets);
                    Person.StartNumsVector(flatBuilder, 3);
                    for (int k = 0; k < 3; ++k)
                    {
                        flatBuilder.AddInt(nums[k]);
                    }
                    var numsOffset = flatBuilder.EndVector();
                    
                    Person.StartMonsVector(flatBuilder, 3);
                    for (int k = 0; k < 3; ++k)
                    {
                        flatBuilder.AddFloat(mons[k]);
                    }
                    var monsOffset = flatBuilder.EndVector();
                    
                    Person.StartYearsVector(flatBuilder, 4);
                    for (int k = 0; k < 4; ++k)
                    {
                        flatBuilder.AddDouble(years[k]);
                    }
                    var yearsOffset = flatBuilder.EndVector();
                    
                    AddressBook.StartAddressBook(flatBuilder);
                    AddressBook.AddId(flatBuilder, 1000);
                    
                    AddressBook.AddAddress(flatBuilder, addressOffset);
                    
                    AddressBook.AddPhones(flatBuilder, vectorOffset);
                    var bookOffset = AddressBook.EndAddressBook(flatBuilder);
                    
                    Person.StartPerson(flatBuilder);
                    Person.AddId(flatBuilder, 100);
                    Person.AddName(flatBuilder, nameOffset);
                    Person.AddAge(flatBuilder, 30);
                    Person.AddNums(flatBuilder, numsOffset);
                    Person.AddMons(flatBuilder, numsOffset);
                    Person.AddYears(flatBuilder, yearsOffset);
                    Person.AddBook(flatBuilder, bookOffset);
                    var p = Person.EndPerson(flatBuilder);
                    
                    flatBuilder.Finish(p.Value);
                    byte[] bytes = flatBuilder.SizedByteArray();
                    file.Write(bytes, 0, bytes.Length);
                }  
                UnityEngine.Profiling.Profiler.EndSample();
            }

            var bts = File.ReadAllBytes(Path.Combine(output, "person.bin"));
            UnityEngine.Profiling.Profiler.BeginSample("TestFlatBufferPref-Deserialize");
            for(int i = 0; i < loopCount; ++i)
            {
                var buf = new ByteBuffer(bts);
                var p = Person.GetRootAsPerson(buf);
            } 
            UnityEngine.Profiling.Profiler.EndSample();   
        }
    }
}