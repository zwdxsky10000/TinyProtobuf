using PGCompiler;
using PGCompiler.Compiler;
using PGCompiler.Compiler.Types;
using PGCompiler.Interfaces;
using PGGenerator;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProtoGen
{
    public static class ProtoGenForUnity
    {
        [MenuItem("Tools/ProtoGen")]
        public static void Generate()
        {
            string protoPath = EditorUtility.OpenFilePanel("选择一个proto文件", null, "proto");
            if(string.IsNullOrEmpty(protoPath))
            {
                EditorUtility.DisplayDialog("警告", "请选择一个正确的proto文件", "确定");
                return;
            }

            string scriptSaveDir = EditorUtility.SaveFolderPanel("选择一个文件夹保存", null, null);
            if (string.IsNullOrEmpty(scriptSaveDir))
            {
                EditorUtility.DisplayDialog("警告", "请选择一个正确文件夹", "确定");
                return;
            }

            string scriptPath = string.Empty; ;
            if (GenCode(protoPath, scriptSaveDir, ref scriptPath))
            {
                Debug.Log("ProtoGen生成脚本文件：" + scriptPath);
                AssetDatabase.Refresh();
            }
        }

        public static bool GenCode(string protoPath, string scriptSaveDir, ref string scriptPath)
        {
            IProtoCompiler compiler = new ProtoCompiler();
            Compilation result = compiler.Compile(protoPath);
            if(result.Errors.Count > 0)
            {
                foreach(var err in result.Errors)
                {
                    Debug.LogError(err.Message);
                }

                return false;
            }
            FileDescriptor descriptor = result.FileDescriptor;
            if(descriptor != null)
            {
                scriptPath = Path.Combine(scriptSaveDir, descriptor.ScriptName);
                return ProtoGenerator.Generator(scriptPath, descriptor);
            }

            return false;
        }
    }
}
