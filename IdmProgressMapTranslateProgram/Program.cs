using System;
using System.IO;

namespace IdmProgressMapTranslateProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string sourcePath = Path.Combine(Environment.CurrentDirectory, @"source\progress map.vsdx");

            string ontologyPath = Path.Combine(Environment.CurrentDirectory, @"source\bpmn2_OWL.owl");

            string newOntologyPath = Path.Combine(Environment.CurrentDirectory, @"source\bpmn2.owl");

            Translator translator = new Translator();

            translator.readOntology(ontologyPath);
            translator.executeTranslation(sourcePath);
            translator.saveOntology(newOntologyPath);

            Console.ReadLine();

        }
    }
}
