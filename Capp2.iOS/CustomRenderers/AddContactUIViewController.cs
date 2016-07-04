﻿using System;
using UIKit;
using Contacts;
using ContactsUI;
using Xamarin.Forms.Platform.iOS;
using Capp2.iOS;
using Xamarin.Forms;
using Capp2;
using System.Linq;
using Foundation;
using Capp2.iOS.Helpers;


[assembly:ExportRenderer(typeof(AddEditContactNativePage), typeof(AddContactUIViewController))]

namespace Capp2.iOS
{
	public class AddContactUIViewController:PageRenderer
	{
		public override void ViewDidLoad ()
		{
			Console.WriteLine ("AddContactUIViewController Viewdidload");
			base.ViewDidLoad ();

			//throw new NotImplementedException ("PresentNativeAddContactView still under construction");
			// Create a new Mutable Contact (read/write)
			// and attach it to the editor
			NSError error;
			var store = new CNContactStore ();
			var fetchKeys = new [] { CNContactKey.FamilyName/*, CNContactKey.FamilyName, CNContactKey.PhoneNumbers*/ }; //contactviewmodel
			var mutableContact = new CNMutableContact ();
			CNMutableContact cnContactToDelete = null, cnContactToAdd = null;
			CNContactViewController editor; 
			var saveRequest = new CNSaveRequest();

			if (ShouldEditContact ()) {
				Console.WriteLine ("ShouldEditContact()");

				//contactviewmodel
				/*mutableContact = store.GetUnifiedContacts 
					(CNContact.GetPredicateForContacts (App.CurrentContact.LastName), fetchKeys, out error)
					//.Where (person => person.GivenName == App.CurrentContact.FirstName &&
					//App.CurrentContact.Number == person.PhoneNumbers [0].Value.StringValue)
					[0].MutableCopy () as CNMutableContact
					;  
				  
				Console.WriteLine("Contact: {0} {1} Number: {2}", mutableContact.GivenName, mutableContact.FamilyName
					, mutableContact.PhoneNumbers[0].Value.StringValue);
				
				if (error != null) { 
					throw new Exception (error.ToString ()); 
				} else {  
					Console.WriteLine ("no error"); 

					Console.WriteLine ("Mutable Contact: {0}", mutableContact.GivenName);*/
 
					cnContactToAdd = new CNMutableContact () {
						GivenName = App.CurrentContact.FirstName,  
						FamilyName = App.CurrentContact.LastName, 
						PhoneNumbers = new CNLabeledValue<CNPhoneNumber>[] {
							new CNLabeledValue<CNPhoneNumber> ("mobile", new CNPhoneNumber (App.CurrentContact.Number)),
							new CNLabeledValue<CNPhoneNumber> ("mobile", new CNPhoneNumber (App.CurrentContact.Number2)),
							new CNLabeledValue<CNPhoneNumber> ("mobile", new CNPhoneNumber (App.CurrentContact.Number3)),
							new CNLabeledValue<CNPhoneNumber> ("mobile", new CNPhoneNumber (App.CurrentContact.Number4)),
							new CNLabeledValue<CNPhoneNumber> ("mobile", new CNPhoneNumber (App.CurrentContact.Number5)),
						},
						OrganizationName = mutableContact.OrganizationName,
					};
					editor = CNContactViewController.FromContact (cnContactToAdd);
					Console.WriteLine ("CNContactViewController.From... done");
					cnContactToDelete = (editor.Contact.MutableCopy () as CNMutableContact);
					
				//}
			} else {
				editor = CNContactViewController.FromNewContact (mutableContact);
			}

			Console.WriteLine ("configuring CNContactViewController");
			// Configure editor
			editor.ContactStore = store;
			editor.AllowsActions = true;
			editor.AllowsEditing = true;
			editor.Delegate = new CNViewControllerDelegate ();
			Console.WriteLine ("done configuring CNContactViewController");

			MessagingCenter.Subscribe<CNViewControllerDelegate>(this, Values.DONEADDINGCONTACTNATIVE, (args) =>
			{
				Console.WriteLine("recieved DONTADDIGNCONTACTNATIVE message");
				PhoneContacts phone = new PhoneContacts();

				/*saveRequest.DeleteContact(cnContactToDelete);
				error = new NSError();
				store.ExecuteSaveRequest(saveRequest, out error);
				if(error != null) Console.WriteLine("Error deleting old contact: {0}", cnContactToDelete.GivenName);
				Console.WriteLine("old contact deleted: {0} {1}", cnContactToDelete.GivenName, cnContactToDelete.FamilyName);
				*/
			});

			// Display picker
			var navcontrol = GetUINavigationController(
					UIApplication.SharedApplication.KeyWindow.RootViewController) as UINavigationController;

			Console.WriteLine("navcontrol is {0}", navcontrol.GetType());
			navcontrol.PushViewController(editor, true);

			//if (App.MasterDetailPage == null)//using TabbedPage as root
			//{
			//	Console.WriteLine("root is TabbedPage");


			/*}
			else { //using MasterDetailPage as root
				Console.WriteLine("root is MasterDetailPage");

				var nav =
					UIApplication.SharedApplication.KeyWindow.RootViewController.ChildViewControllers.First().
					ChildViewControllers.Last().ChildViewControllers.First();
				
				var navcontrol = GetUINavigationController(
					UIApplication.SharedApplication.KeyWindow.RootViewController) as UINavigationController;
					//nav as UINavigationController;

				Console.WriteLine("navcontrol is {0}", navcontrol.GetType());
				navcontrol.PushViewController(editor, true);
			}*/

			Console.WriteLine ("Done w function");
		}

		UINavigationController GetUINavigationController(UIViewController controller) {
			if (controller != null) {
				Console.WriteLine("controller is not null");
				if (IsUINavigationViewController(controller))
				{
					Console.WriteLine("Found uinavigationcontroller");
					return (controller as UINavigationController);
				}

				if (controller.ChildViewControllers.Count() != 0)
				{
					var count = controller.ChildViewControllers.Count();

					for (int c = 0; c < count; c++)
					{
						Console.WriteLine(
							"local iteration {0}: current controller has {1} children", c, count);
						var child = GetUINavigationController(controller.ChildViewControllers[c]);
						if (child == null)
						{
							Console.WriteLine("No children left on current controller. Moving back up");
						}
						else if(IsUINavigationViewController(child)){
							Console.WriteLine("returning customnavigationrenderer");
							return (child as UINavigationController);
						}
					}
				}
			}

			return null;
		}

		bool IsUINavigationViewController(NSObject view) {
			if (view.GetType() == new CustomNavigationRenderer().GetType())
				return true;
			return false;
		}

		bool ShouldEditContact(){
			return (App.CurrentContact == null) ? false : true;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			Console.WriteLine ("AddContactUIViewController viewdidappear");
		}
	}
}

