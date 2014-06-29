using System;
using System.IO;

namespace IdmProgressMapTranslateProgram
{
    public class Program
    {
        //http://www.omg.org/spec/BPMN/20100501/Semantic.xsd
        public static void Main(string[] args)
        {

            string sourcePath = Path.Combine(Environment.CurrentDirectory, @"source\progress map.vsdx");

            Translator translator = new Translator();
            translator.execute(sourcePath);

            Console.ReadLine();

        }
    }
}
