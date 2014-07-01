/*****************************************************************************
 * IOwlGenerator.cs
 * 
 * Copyright (c) 2005, Bram Pellens.
 *  
 * This file is part of the OwlDotNetApi.
 * The OwlDotNetApi is free software; you can redistribute it and/or 
 * modify it under the terms of the GNU Lesser General Public License as published 
 * by the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * The OwlDotNetApi is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with the OwlDotNetApi; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * Author: 
 * 
 * Bram Pellens
 * bpellens@gmail.com
 ******************************************************************************/

using System;
using System.Collections;
using System.IO;

namespace OwlDotNetApi
{
	/// <summary>
	/// Represents an OWL Generator
	/// </summary>
	public interface IOwlGenerator
	{
		/// <summary>
		/// Indicates whether the generator should throw an exception and stop when it encounters an error
		/// </summary>
		bool StopOnErrors
		{
			get;
			set;
		}
		
		/// <summary>
		/// Indicates whether the generator should throw an exception and stop when it encounters a warning
		/// </summary>
		bool StopOnWarnings
		{
			get;
			set;
		}

		/// <summary>
		/// Represents a list of warning messages generated by the generator
		/// </summary>
		ArrayList Warnings
		{
			get;
		}

		/// <summary>
		/// Represents a list of error messages generated by the generator
		/// </summary>
		ArrayList Errors
		{
			get;
		}

		/// <summary>
		/// Represents a list of logging messages
		/// </summary>
		ArrayList Messages
		{
			get;
		}

		/// <summary>
		/// When implemented by a class, it generates the graph to a file given by the uri.
		/// </summary>
		/// <param name="graph">The owl graph the needs to be generated</param>
		/// <param name="uri">The uri representing the file that will be used as a destination of the graph</param>
		void GenerateOwl(IOwlGraph graph, string uri);

		/// <summary>
		/// When implemented by a class, it generates the graph to a file given by the uri.
		/// </summary>
		/// <param name="graph">The owl graph the needs to be generated</param>
		/// <param name="uri">The object of type Uri representing the file that will be used as a destination of the graph</param>
		void GenerateOwl(IOwlGraph graph, Uri uri);
	}
}