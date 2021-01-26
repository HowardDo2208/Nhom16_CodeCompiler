using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CodeOnlineBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompileController : ApiController
    {
        public CompileController()
        {
            ExecuteCommandSync("docker start codecompile");
            //khi construct sẽ chạy docker
        }

        public List<string> ExecuteCommandSync(object command)
        {
            var result = new List<string>();
            var errors = new List<string>();
            System.Diagnostics.ProcessStartInfo processStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = processStartInfo;
            var a = proc.Start();
            while (!proc.StandardError.EndOfStream)
            {
                var line = proc.StandardError.ReadLine();
                errors.Add(line);
            }
            if (errors.Count() > 0)
                return errors;
            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                result.Add(line);
            }
            return result;
        }

        [Route("api/compile/java")]
        [HttpPost]
        public List<string> Java([FromBody] Request request)
        {
            string filePath = @"E:\projects\CodeOnline\src\Main.java";
            StreamWriter writer = new StreamWriter(filePath);
            try
            {
                using (writer)
                {
                    writer.Write(request.codeText);
                }
            }
            catch (Exception exp)
            {
                var problem = new List<string>();
                problem.Add("there is a problem while writing file");
                return problem;
            }

            string command = "docker exec codecompile bash /data/compile.sh";
            var result = ExecuteCommandSync(command);
            return result;
        }

        [Route("api/compile/csharp")]
        [HttpPost]
        public List<string> CSharp([FromBody] Request request)
        {
            string filePath = @"E:\projects\CodeOnline\src\Main.cs";
            StreamWriter writer = new StreamWriter(filePath);
            try
            {
                using (writer)
                {
                    writer.Write(request.codeText);
                }
            }
            catch (Exception exp)
            {
                var problem = new List<string>();
                problem.Add("there is a problem while writing file");
                return problem;
            }

            string command = "docker exec codecompile bash /data/compileC.sh";
            var result = ExecuteCommandSync(command);
            return result;
        }

        [Route("api/compile/exercise1")]
        [HttpPost]
        public bool exercise1([FromBody] ExerciseRequest request)
        {
            //test case
            int[,] testValues = {
            { 1, 2},
            { 2, 4},
        };
            var language = request.language;

            if (language == "java")
            {
                //kiểm tra test case khi ngôn ngữ là java
                string filePath = @"E:\projects\CodeOnline\src\Main.java";
                StreamWriter writer = new StreamWriter(filePath);
                using (writer)
                {
                    writer.Write(request.codeText);
                }
                string command = "docker exec codecompile bash /data/compile.sh";
                var result1 = ExecuteCommandSync(command + " " + testValues[0, 0])[0];
                if (result1 != testValues[0, 1].ToString())
                    return false;
                var result2 = ExecuteCommandSync(command + " " + testValues[1, 0])[0];
                if (result2 != testValues[1, 1].ToString())
                    return false;
                return true;
            }
            else
            {
                //kiểm tra test case khi ngôn ngữ là csharp
                string filePath = @"E:\projects\CodeOnline\src\Main.cs";
                StreamWriter writer = new StreamWriter(filePath);
                using (writer)
                {
                    writer.Write(request.codeText);
                }
                string command = "docker exec codecompile bash /data/compileC.sh";
                var result1 = ExecuteCommandSync(command + " " + testValues[0, 0])[0];
                if (result1 != testValues[0, 1].ToString())
                    return false;
                var result2 = ExecuteCommandSync(command + " " + testValues[1, 0])[0];
                if ( result2 != testValues[1, 1].ToString())
                    return false;
                return true;
            }
        }

        [Route("api/compile/exercise2")]
        [HttpPost]
        public bool exercise2([FromBody] ExerciseRequest request)
        {
            int[,] testValues = {
            { 10, 100},
            { 9, 81},
        };
            var language = request.language;

            if (language == "java")
            {
                string filePath = @"E:\projects\CodeOnline\src\Main.java";
                StreamWriter writer = new StreamWriter(filePath);
                using (writer)
                {
                    writer.Write(request.codeText);
                }
                string command = "docker exec codecompile bash /data/compile.sh";
                var result1 = ExecuteCommandSync(command + " " + testValues[0, 0])[0];
                if (result1 != testValues[0, 1].ToString())
                    return false;
                var result2 = ExecuteCommandSync(command + " " + testValues[1, 0])[0];
                if (result2 != testValues[1, 1].ToString())
                    return false;
                return true;
            }
            else
            {
                string filePath = @"E:\projects\CodeOnline\src\Main.cs";
                StreamWriter writer = new StreamWriter(filePath);
                using (writer)
                {
                    writer.Write(request.codeText);
                }
                string command = "docker exec codecompile bash /data/compileC.sh";
                var result1 = ExecuteCommandSync(command + " " + testValues[0, 0])[0];
                if (result1 != testValues[0, 1].ToString())
                    return false;
                var result2 = ExecuteCommandSync(command + " " + testValues[1, 0])[0];
                if (result2 != testValues[1, 1].ToString())
                    return false;
                return true;
            }
        }
    }

    public class Request
    {
        public string codeText{get; set;}
    }
    public class ExerciseRequest
    {
        public string language { get; set; }
        public string codeText { get; set; }
    }
}
