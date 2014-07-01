using System;
using System.IO;

namespace IdmProgressMapTranslateProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string progressMapPath = Path.Combine(Environment.CurrentDirectory, @"source\progress map.vsdx");
            string inputOntologyPath = Path.Combine(Environment.CurrentDirectory, @"source\bpmn2_OWL.owl");
            string outputOntologyPath = Path.Combine(Environment.CurrentDirectory, @"source\bpmn2.owl");

            Translator translator = new Translator();

            translator.InputOntologyPath = inputOntologyPath;
            translator.ProgressMapPath = progressMapPath;
            translator.OutputOntologyPath = outputOntologyPath;

            translator.execute();

            Console.ReadLine();

        }
    }
}
