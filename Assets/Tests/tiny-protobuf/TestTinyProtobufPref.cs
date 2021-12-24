using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestTinyProtobufPref : MonoBehaviour
{
    public int loopCount = 1000;

    // Start is called before the first frame update
    void Start()
    {
        string output = Application.dataPath + "/../";
        TestPref(output);
    }

    void TestPref(string output)
    {
        tinyprotobuf.Person p = new tinyprotobuf.Person();
        {
            p.id = 100;
            p.name = "toyqiu";
            p.age = 30;
            p.nums = new List<int>(){12,34,56};
            p.mons = new List<float>(){4.3f, 5.4f, 6.5f};
            p.years = new List<double>(){5,4,4,2};
            p.book = new tinyprotobuf.AddressBook();
            {
                p.book.id = 1000;
                p.book.address = "shenzhen,nanshan";
                p.book.phones.Add(new tinyprotobuf.AddressPhone()
                    {
                        id = 10000,
                        phone = "13141231212",
                        email = "13123@qq.com",
                        serial = 3.1415923,
                        cost = 1.0f
                    });
                p.book.phones.Add(new tinyprotobuf.AddressPhone()
                    {
                        id = 10000,
                        phone = "13141231212",
                        email = "13123@qq.com",
                        serial = 3.1415923,
                        cost = 1.0f
                    });
                p.book.phones.Add(new tinyprotobuf.AddressPhone()
                    {
                        id = 10001,
                        phone = "13141231215",
                        email = "13123323@qq.com",
                        serial = 3.1415955,
                        cost = 1.9f
                    });
                p.book.phones.Add(new tinyprotobuf.AddressPhone()
                    {
                        id = 10002,
                        phone = "13141231213",
                        email = "1312353@qq.com",
                        serial = 3.14159213,
                        cost = 1.6f
                    });
                p.book.phones.Add(new tinyprotobuf.AddressPhone()
                    {
                        id = 10004,
                        phone = "13141231218",
                        email = "1312313@qq.com",
                        serial = 3.1415966,
                        cost = 1.1f
                    });
                p.book.phones.Add(new tinyprotobuf.AddressPhone()
                    {
                        id = 10003,
                        phone = "13141231217",
                        email = "1312363@qq.com",
                        serial = 3.141594343,
                        cost = 1.3f
                    });
            }
        };    
        
        using (var stream = File.Open(Path.Combine(output, "person.bin"), FileMode.OpenOrCreate))
        {
            UnityEngine.Profiling.Profiler.BeginSample("TestTinyProtobufPref-Serialize");
            for(int i = 0; i < loopCount; ++i)
            {
                var outputStream = Protobuf.StreamPool.GetOutputStream();

                p.Encode(outputStream);

                outputStream.FlushToStream(stream);
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }

        using (var stream = File.OpenRead(Path.Combine(output, "person.bin")))
        {
            UnityEngine.Profiling.Profiler.BeginSample("TestProtobufnetPref-Deserialize");
            for(int i = 0; i < loopCount; ++i)
            {
                var input = Protobuf.StreamPool.GetInputStream(stream);
                tinyprotobuf.Person person1 = new tinyprotobuf.Person();
                person1.Decode(input);
            } 
            UnityEngine.Profiling.Profiler.EndSample();
            
            //Debug.LogFormat("id=" + person1.id);
        }
    }
}

