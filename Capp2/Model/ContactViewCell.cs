﻿using System;
using Xamarin.Forms;
using System.Diagnostics;
using XLabs.Forms.Controls;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;

namespace Capp2
{
	public class ContactViewCell:ViewCell
	{
		public ContactData personCalled{ set; get;}
		TapGestureRecognizer tapGestureRecognizer;
		Label nameLabel;
		Label playlistLabel;
		Image phone;
		CheckBox checkbox;

		public ContactViewCell (CAPP page)
		{
			this.Height = 56;
			nameLabel = new Label{
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalTextAlignment = TextAlignment.Start,
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");//"Name" links directly to the ContactData.Name property

			playlistLabel = new Label{
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)), 
				TextColor = Color.Green,
				VerticalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Start,
			};
			playlistLabel.SetBinding(Label.TextProperty, "Playlist");//"Playlist" links directly to the ContactData.Name property

			checkbox = new CheckBox{ 
				HorizontalOptions = LayoutOptions.Center
			};
            checkbox.IsEnabled = true;
            checkbox.IsVisible = true;
            
			checkbox.SetBinding (CheckBox.CheckedProperty, "IsSelected");
			checkbox.CheckedChanged += (sender, e) => {
				personCalled = (sender as CheckBox).Parent.Parent.Parent.BindingContext as ContactData;
				Debug.WriteLine (personCalled.Name+"' selected value is "+personCalled.IsSelected.ToString ());
				App.Database.UpdateItem(personCalled);

				var person = (from x in (App.Database.GetItems (Values.ALLPLAYLISTPARAM).Where(x => x.Name == personCalled.Name))
					select x);
				Debug.WriteLine (person.ElementAtOrDefault (0).Name+"' selected value is "+person.ElementAtOrDefault (0).IsSelected.ToString ()); 
			};

			phone = new Image {
				Aspect = Aspect.AspectFit,
				Source = FileImageSource.FromFile ("Phone"),
				HorizontalOptions = LayoutOptions.End
			};
			tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				personCalled = (s as Image).Parent.Parent.Parent.BindingContext as ContactData;
				page.call(personCalled, false);
			};
			phone.GestureRecognizers.Add (tapGestureRecognizer);
			tapGestureRecognizer.NumberOfTapsRequired = 1;

			var nextAction = new MenuItem { Text = "Next Meeting" };
			nextAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			nextAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				personCalled = (ContactData)mi.BindingContext;

                if (!personCalled.Appointed.ToString().Contains("1/1/0001"))
                {
                    page.Navigation.PushModalAsync(new DatePage(Values.NEXT, personCalled, false));//pass Contact.ID of listview row selected via contextmenu event listener
                }
                else {
                    Debug.WriteLine("Set an appointment for {0} first", personCalled.FirstName);
                    UserDialogs.Instance.WarnToast("Pls book "+ personCalled.FirstName + " at a BOM before we can move on to the follow up!", null, 2000);
                }
            };

			var appointedAction = new MenuItem { Text = "Appointed" };
			appointedAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			appointedAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				personCalled = (ContactData)mi.BindingContext;
				page.Navigation.PushModalAsync(new DatePage(Values.APPOINTED, personCalled, false));//pass Contact.ID of listview row selected via contextmenu event listener
			};

			var presentedAction = new MenuItem { Text = "Presented" };
			presentedAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			presentedAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				personCalled = (ContactData)mi.BindingContext;
				page.Navigation.PushModalAsync(new DatePage(Values.PRESENTED, personCalled, false));
			};

			var purchasedAction = new MenuItem { Text = "Purchased" }; 
			purchasedAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			purchasedAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				personCalled = (ContactData)mi.BindingContext;
				page.Navigation.PushModalAsync(new DatePage(Values.PURCHASED, personCalled, false));
			};

			View = createView (page.playlistChosen.PlaylistName);

			// add context actions to the cell
			ContextActions.Add(nextAction);
			ContextActions.Add (appointedAction);
			ContextActions.Add (presentedAction);
			ContextActions.Add (purchasedAction);
		}

		public View createView (string playlist)
		{
			if (App.IsEditing) {
				if (string.Equals (playlist, Values.ALLPLAYLISTPARAM)) {
					return new StackLayout {
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Padding = new Thickness (15, 5, 5, 15),
						Children = { new StackLayout {
								Orientation = StackOrientation.Vertical,
								HorizontalOptions = LayoutOptions.Start,
								Children = { nameLabel, playlistLabel }
							}, new StackLayout {
								Orientation = StackOrientation.Horizontal,
								HorizontalOptions = LayoutOptions.EndAndExpand,
								Children = { checkbox }
							}
						}
					};
				} else {
					return new StackLayout {
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Padding = new Thickness (15, 5, 5, 15),
						Children = {
							nameLabel, 
							new StackLayout {
								Orientation = StackOrientation.Horizontal,
								HorizontalOptions = LayoutOptions.EndAndExpand,
								Children = { checkbox }
							}
						}
					};
				}
			} else {
				if (string.Equals (playlist, Values.ALLPLAYLISTPARAM)) {
					return new StackLayout {
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Padding = new Thickness (15, 5, 5, 15),
						Children = { new StackLayout {
								Orientation = StackOrientation.Vertical,
								HorizontalOptions = LayoutOptions.Start,
								Children = { nameLabel, playlistLabel }
							}, new StackLayout {
								Orientation = StackOrientation.Horizontal,
								HorizontalOptions = LayoutOptions.EndAndExpand,
								Children = { phone }
							}
						}
					};
				} else {
					return new StackLayout {
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Padding = new Thickness (15, 5, 5, 15),
						Children = {
							nameLabel, 
							new StackLayout {
								Orientation = StackOrientation.Horizontal,
								HorizontalOptions = LayoutOptions.EndAndExpand,
								Children = { phone }
							}
						}
					};
				}
			}
		}


	}
}

