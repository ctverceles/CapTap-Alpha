﻿using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace Capp2
{
	public class NativeCell : ViewCell
	{
		public NativeCell():base()
		{
		}

		public static readonly BindableProperty NameProperty =
			BindableProperty.Create ("Name", typeof(string), typeof(NativeCell), "");

		public string Name {
			get { return (string)GetValue (NameProperty); }
			set { SetValue (NameProperty, value); }
		}

		public static readonly BindableProperty CategoryProperty =
			BindableProperty.Create ("Category", typeof(string), typeof(NativeCell), "");

		public string Category {
			get { return (string)GetValue (CategoryProperty); }
			set { SetValue (CategoryProperty, value); }
		}

		public static readonly BindableProperty ImageFilenameProperty =
			BindableProperty.Create ("ImageFilename", typeof(string), typeof(NativeCell), "");

		public string ImageFilename {
			get { return (string)GetValue (ImageFilenameProperty); }
			set { SetValue (ImageFilenameProperty, value); }
		}


	}
}

