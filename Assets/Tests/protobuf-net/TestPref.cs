using UnityEngine;
using System.IO;

public class TestPref : MonoBehaviour
{
    public int loopCount = 1000;

    // Start is called before the first frame update
    void Start()
    {
        string output = Application.dataPath + "/../";
        TestProtobufnetPref(output);
    }

    void TestProtobufnetPref(string output)
    {
        protobufnet.Person p = new protobufnet.Person();
        {
            p.Id = 100;
            p.Name = "toyqiu";
            p.Age = 30;
            p.Nums = new int[]{12,34,56};
            p.Mons = new float[]{4.3f, 5.4f, 6.5f};
            p.Years = new double[]{5,4,4,2};
            p.Book = new protobufnet.AddressBook();
            {
                p.Book.Id = 1000;
                p.Book.Address = "shenzhen,nanshan";
                p.Book.Phones.Add(new protobufnet.AddressPhone()
                    {
                        Id = 10000,
                        Phone = "13141231212",
                        Email = "13123@qq.com",
                        Serial = 3.1415923,
                        Cost = 1.0f
                    });
                p.Book.Phones.Add(new protobufnet.AddressPhone()
                    {
                        Id = 10000,
                        Phone = "13141231212",
                        Email = "13123@qq.com",
                        Serial = 3.1415923,
                        Cost = 1.0f
                    });
                p.Book.Phones.Add(new protobufnet.AddressPhone()
                    {
                        Id = 10001,
                        Phone = "13141231215",
                        Email = "13123323@qq.com",
                        Serial = 3.1415955,
                        Cost = 1.9f
                    });
                p.Book.Phones.Add(new protobufnet.AddressPhone()
                    {
                        Id = 10002,
                        Phone = "13141231213",
                        Email = "1312353@qq.com",
                        Serial = 3.14159213,
                        Cost = 1.6f
                    });
                p.Book.Phones.Add(new protobufnet.AddressPhone()
                    {
                        Id = 10004,
                        Phone = "13141231218",
                        Email = "1312313@qq.com",
                        Serial = 3.1415966,
                        Cost = 1.1f
                    });
                p.Book.Phones.Add(new protobufnet.AddressPhone()
                    {
                        Id = 10003,
                        Phone = "13141231217",
                        Email = "1312363@qq.com",
                        Serial = 3.141594343,
                        Cost = 1.3f
                    });
            }
        };
        
        using (var file = File.Create(Path.Combine(output, "person.bin"))) {
            UnityEngine.Profiling.Profiler.BeginSample("TestProtobufnetPref-Serialize");
            for(int i = 0; i < loopCount; ++i)
            {
                ProtoBuf.Serializer.Serialize(file, p);
            }  
            UnityEngine.Profiling.Profiler.EndSample();
        }

        protobufnet.Person newPerson;
        using (var file = File.OpenRead(Path.Combine(output, "person.bin"))) {
            UnityEngine.Profiling.Profiler.BeginSample("TestProtobufnetPref-Deserialize");
            for(int i = 0; i < loopCount; ++i)
            {
                newPerson = ProtoBuf.Serializer.Deserialize<protobufnet.Person>(file);
            } 
            UnityEngine.Profiling.Profiler.EndSample();
        }       
    }
}
