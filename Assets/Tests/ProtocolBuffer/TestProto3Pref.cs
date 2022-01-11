using UnityEngine;
using System.IO;
using Google.Protobuf;

public class TestProto3Pref : MonoBehaviour
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
        Google.Protobuf3.Person p = new Google.Protobuf3.Person();
        {
            p.Id = 100;
            p.Name = "toyqiu";
            p.Age = 30;
            p.Nums.AddRange(new int[] { 12, 34, 56 });
            p.Mons.AddRange(new float[] { 4.3f, 5.4f, 6.5f });
            p.Years.AddRange(new double[] { 5, 4, 4, 2 });
            p.Book = new Google.Protobuf3.AddressBook();
            {
                p.Book.Id = 1000;
                p.Book.Address = "shenzhen,nanshan";
                p.Book.Phones.Add(new Google.Protobuf3.AddressPhone()
                {
                    Id = 10000,
                    Phone = "13141231212",
                    Email = "13123@qq.com",
                    Serial = 3.1415923,
                    Cost = 1.0f
                });
                p.Book.Phones.Add(new Google.Protobuf3.AddressPhone()
                {
                    Id = 10000,
                    Phone = "13141231212",
                    Email = "13123@qq.com",
                    Serial = 3.1415923,
                    Cost = 1.0f
                });
                p.Book.Phones.Add(new Google.Protobuf3.AddressPhone()
                {
                    Id = 10001,
                    Phone = "13141231215",
                    Email = "13123323@qq.com",
                    Serial = 3.1415955,
                    Cost = 1.9f
                });
                p.Book.Phones.Add(new Google.Protobuf3.AddressPhone()
                {
                    Id = 10002,
                    Phone = "13141231213",
                    Email = "1312353@qq.com",
                    Serial = 3.14159213,
                    Cost = 1.6f
                });
                p.Book.Phones.Add(new Google.Protobuf3.AddressPhone()
                {
                    Id = 10004,
                    Phone = "13141231218",
                    Email = "1312313@qq.com",
                    Serial = 3.1415966,
                    Cost = 1.1f
                });
                p.Book.Phones.Add(new Google.Protobuf3.AddressPhone()
                {
                    Id = 10003,
                    Phone = "13141231217",
                    Email = "1312363@qq.com",
                    Serial = 3.141594343,
                    Cost = 1.3f
                });
            }
        };

        using (var file = File.Create(Path.Combine(output, "person3.bin")))
        {
            UnityEngine.Profiling.Profiler.BeginSample("TestProtobuf3Pref-Serialize");
            for (int i = 0; i < loopCount; ++i)
            {
                p.WriteTo(file);
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }

        Google.Protobuf3.Person newPerson;
        using (var file = File.OpenRead(Path.Combine(output, "person3.bin")))
        {
            UnityEngine.Profiling.Profiler.BeginSample("TestProtobuf3Pref-Deserialize");
            for (int i = 0; i < loopCount; ++i)
            {
                newPerson = Google.Protobuf3.Person.Parser.ParseFrom(file);
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}
