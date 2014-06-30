using System;
using System.IO;
using System.Collections;
using OwlDotNetApi;

namespace OwlDotNetApiTest
{
	/// <summary>
	/// Summary description for Test.
	/// </summary>
	class Test
	{

		public Test() 
		{

		}

		public void test1(string file) 
		{
			IOwlParser parser = new OwlXmlParser();

			IOwlGraph graph = parser.ParseOwl(file);
			ArrayList errors = ((OwlParser)parser).Errors;
			ArrayList warnings = ((OwlParser)parser).Warnings;
			ArrayList messages = ((OwlParser)parser).Messages;

			FileStream info = new FileStream("c:/info.txt", FileMode.OpenOrCreate);
			StreamWriter sw = new StreamWriter(info);
			sw.AutoFlush = true;

			foreach(string msg in messages)
			{
				Console.WriteLine(msg);
			}

			//IOwlGenerator generator = new OwlXmlGenerator();
			//generator.GenerateOwl(graph, @"c:\example1.owl");

			info = new FileStream("C:/generated.txt", FileMode.OpenOrCreate);
			sw = new StreamWriter(info);
			sw.AutoFlush = true;

			//messages = ((OwlGenerator)generator).Messages;

			//foreach(string msg in messages)
			//{
			//	sw.WriteLine(msg);
			//}

			Console.WriteLine("Graph parsed successfully with {0} errors and {1} warnings\n\n",errors.Count,warnings.Count);

			foreach(string err in errors)
			{
				Console.WriteLine("Error: "+err);
			}

			foreach(string war in warnings)
			{
				Console.WriteLine("Warning: "+war);
			}

			Console.WriteLine("The graph contains {0} node(s) and {1} edge(s).", graph.Nodes.Count, graph.Edges.Count);
		}

		public void test2() 
		{
			OwlGraph ontology = new OwlGraph();
			ontology.NameSpaces["xmlns:" + OwlNamespaceCollection.OwlNamespacePrefix] = OwlNamespaceCollection.OwlNamespace;
			ontology.NameSpaces["xmlns:" + OwlNamespaceCollection.RdfSchemaNamespacePrefix] = OwlNamespaceCollection.RdfSchemaNamespace;
			ontology.NameSpaces["xmlns:daml"] = "http://www.daml.org/2001/03/daml+oil#";
			ontology.NameSpaces["xmlns:dc"] = "http://purl.org/dc/elements/1.1/";
			ontology.NameSpaces["xmlns"] = "http://www.owl-ontologies.com/test.owl#";
			ontology.NameSpaces["xml:base"] = "http://www.owl-ontologies.com/test.owl";

			string baseUri = "http://www.owl-ontologies.com/test.owl#";

			OwlOntology o = new OwlOntology(baseUri + "testOntology");
			ontology.Nodes.Add(o);

			OwlClass a = new OwlClass(baseUri + "ClassA");
			ontology.Nodes.Add(a);

			OwlClass b = new OwlClass(baseUri + "ClassB");
			ontology.Nodes.Add(b);

			OwlEdge relation = new OwlEdge(OwlNamespaceCollection.RdfSchemaNamespace + "subClassOf");
			relation.AttachParentNode(a);
			relation.AttachChildNode(b);
			ontology.Edges.Add(relation);

			IOwlGenerator generator = new OwlXmlGenerator();
			generator.GenerateOwl(ontology, @"c:\example2.owl");
		}

		public void test3(string file) 
		{
			IOwlParser parser = new OwlXmlParser();
			IOwlGraph graph = parser.ParseOwl(file);

			Console.WriteLine("The nodes of the graph are:");
			IDictionaryEnumerator nEnumerator = (IDictionaryEnumerator)graph.Nodes.GetEnumerator();
			while(nEnumerator.MoveNext()) 
			{
				OwlNode node = (OwlNode)graph.Nodes[(nEnumerator.Key).ToString()];
				if(!node.IsAnonymous()) Console.WriteLine(node.ID);
			}

			Console.WriteLine(Environment.NewLine);

			Console.WriteLine("Retrieving some specific data:");
			IOwlNode hotelNode = (IOwlNode)graph.Nodes["http://www.owl-ontologies.com/travel.owl#Hotel"];
			
			Console.WriteLine(Environment.NewLine);
			Console.WriteLine("The edges are: ");
			OwlEdgeCollection edges = (OwlEdgeCollection)hotelNode.ChildEdges;
			foreach(OwlEdge e in edges) 
			{
				Console.WriteLine(e.ID);
			}

			Console.WriteLine("The subClassOf edges are:");
			IOwlEdgeList subclassEdges = (IOwlEdgeList)hotelNode.ChildEdges["http://www.w3.org/2000/01/rdf-schema#subClassOf"];
			foreach(OwlEdge s in subclassEdges) 
			{
				Console.WriteLine(s.ChildNode.ID);
			}
		}

		public void test4(string file) 
		{
			IOwlParser parser = new OwlXmlParser();
			IOwlGraph graph = parser.ParseOwl(file);

			Console.WriteLine("Retrieving some specific data:");
		
			// Here we will retrieve the enumerator in order to get all the nodes from the file
			IDictionaryEnumerator nEnumerator = (IDictionaryEnumerator)graph.Nodes.GetEnumerator();
			while(nEnumerator.MoveNext()) 
			{
				// Get the node from the graph
				OwlNode node = (OwlNode)graph.Nodes[(nEnumerator.Key).ToString()];
				// We will cast the node to a OwlClass because we are looking for classes
				OwlClass clsNode = node as OwlClass;
				// If clsNode is different from null, then we are dealing with an OwlClass -> OK
				// If the clsNode is not anonymous, means that we have a class with a proper name -> OK
				if((clsNode != null) && (!clsNode.IsAnonymous())) 
				{
					// So, now we have a good owl-class, we will look for any subClassOf relations (edges)
					IOwlEdgeList subclassEdges = (IOwlEdgeList)node.ChildEdges["http://www.w3.org/2000/01/rdf-schema#subClassOf"];
					if(subclassEdges != null) 
					{
						// We will list all the edges and check if the target of the edge is the class we want to
						// have as the superclass
						foreach(OwlEdge s in subclassEdges) 
						{
							if(s.ChildNode.ID == "http://www.owl-ontologies.com/travel.owl#Accommodation")
								Console.WriteLine(node.ID);
						}
					}
				}
			}
		}

		public void test5(string file) 
		{
			IOwlParser parser = new OwlXmlParser();
			IOwlGraph graph = parser.ParseOwl(file);

			string baseUri = "http://www.owl-ontologies.com/travel.owl#";
			OwlClass hotelNode = (OwlClass)graph.Nodes["http://www.owl-ontologies.com/travel.owl#LuxuryHotel"];

			OwlIndividual newHotel = new OwlIndividual(baseUri + "PellensPalace", hotelNode);
			graph.Nodes.Add(newHotel);

			IOwlGenerator generator = new OwlXmlGenerator();
			generator.GenerateOwl(graph, @"c:\travelnew.owl");
		}

		public void test6(string file) 
		{
			IOwlParser parser = new OwlXmlParser();
			IOwlGraph graph = parser.ParseOwl(file);

			IOwlGenerator generator = new OwlXmlGenerator();
			generator.GenerateOwl(graph, @"c:\travelnew.owl");
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Test t = new Test();

			// First Example:
			// Reading an ontology from a file and afterwards writing it again to another file
			//t.test1(args[0]); 

			// Second Example:
			// Creating a new ontology with a two classes and making one class a subclass of
			// another class and finally writing it to file
			//t.test2();

			// Third Example:
			// Reading an ontology from a file and retrieving some information from it
			//t.test3("C:\\travel.owl");

			t.test1("C:\\final2.owl");
		}
	}
}
