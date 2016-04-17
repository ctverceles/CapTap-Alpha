﻿using System;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Plugin.Calendars.Abstractions;
using System.Threading.Tasks;

namespace Capp2
{
	public class PlaylistPage:ContentPage//MasterDetailPage
	{
		public ListView listView{ get; set;}
		public Playlist playlistSelected;

		public PlaylistPage ()
		{
			this.Title = "Namelists";
			this.BackgroundColor = Color.White;

			var searchBar = new SearchBar {
				Placeholder = "Enter a namelist",
			};
			searchBar.TextChanged += (sender, e) => {
				FilterPlaylists(searchBar.Text);
			};

			//just to initialize the database to avoid nullreferenceexceptions
			PlaylistDB playlists = App.Playlists;

			listView = new ListView{
				ItemsSource = App.Playlists.GetItems(),
				ItemTemplate = new DataTemplate(() => 
					{
						return new PlaylistViewCell(this);
					})
			};
			listView.ItemSelected += (sender, e) => {
				this.playlistSelected = (Playlist)e.SelectedItem;

				// has been set to null, do not 'process' tapped event
				if (e.SelectedItem == null)
					return; 

				//load contacts based on type of playlist (warm, cold, semi warm whatever playlist is tapped)
				Navigation.PushAsync(new CAPP(playlistSelected));

				// de-select the row
				((ListView)sender).SelectedItem = null; 
			};

			var stack = new StackLayout {
			Padding = new Thickness(10),
			Children = {
				searchBar, 
				new StackLayout{
					Padding = new Thickness(7,0,0,0),
					Children = {listView}
					}
				}
			};

			Content = UIBuilder.AddFloatingActionButtonToStackLayout(stack, "ic_add_white_24dp.png", new Command (async () =>
				{
					var result = await UserDialogs.Instance.PromptAsync("Please enter a name for this list:", "New namelist", "OK", "Cancel");
					if(string.IsNullOrWhiteSpace(result.Text) || string.IsNullOrEmpty(result.Text)){
					}else {
						App.Playlists.SaveItem(new Playlist{PlaylistName = result.Text});
						refresh();
					}
				}), Color.FromHex (Values.GOOGLEBLUE), Color.FromHex (Values.PURPLE));
		}

		public void refresh ()
		{
			listView.ItemsSource = App.Playlists.GetItems ();
		}

		public void FilterPlaylists(string filter)
		{
			listView.BeginRefresh ();

			if (string.IsNullOrWhiteSpace (filter)) {
				refresh();
			} else {
				listView.ItemsSource = App.Playlists.GetItems()
					.Where (x => x.PlaylistName.ToLower ()
						.Contains (filter.ToLower ()));
			}

			listView.EndRefresh ();
		}
	}

	public class PlaylistViewCell:ViewCell
	{
		public PlaylistViewCell (PlaylistPage page)
		{
			Label playlistLabel = new Label();
			playlistLabel.SetBinding(Label.TextProperty, "PlaylistName");//"Name" binds directly to the ContactData.Name property

			var EditAction = new MenuItem { Text = "Edit" };
			EditAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			EditAction.Clicked += async (sender, e) => {
				var mi = ((MenuItem)sender);
				await EditPlaylistName((Playlist)mi.BindingContext);
				page.refresh ();
			};

			var DeleteAction = new MenuItem { Text = "Delete" , IsDestructive = true};
			DeleteAction.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			DeleteAction.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				DeletePlaylist((Playlist)mi.BindingContext);
				page.refresh ();
			};

			View = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness (15, 5, 5, 15),
				Children = { playlistLabel }
			};

			ContextActions.Add(EditAction);
			ContextActions.Add(DeleteAction);
		}
		async Task<bool> EditPlaylistName(Playlist playlist){
			if (string.Equals (Values.ALLPLAYLISTPARAM, playlist.PlaylistName) || string.Equals (Values.TODAYSCALLS, playlist.PlaylistName)) {
				UserDialogs.Instance.InfoToast ("Sorry, we can't edit an essential namelist");
			} else {
				var result = await UserDialogs.Instance.PromptAsync("Enter a new name for this namelist:", "", "OK", "Cancel");
				if (string.IsNullOrWhiteSpace (result.Text) || string.IsNullOrEmpty (result.Text)) {
				} else {
					playlist.PlaylistName = result.Text;
					App.Playlists.UpdateItem(playlist);//how to update all contacts playlist property? data bind?
					return true;
				}
			}
			return false;
		}
		void DeletePlaylist(Playlist playlist){
			if (string.Equals (Values.ALLPLAYLISTPARAM, playlist.PlaylistName) || string.Equals (Values.TODAYSCALLS, playlist.PlaylistName)) {
				UserDialogs.Instance.InfoToast ("Sorry, we can't delete an essential namelist");
			} else {
				App.Playlists.DeleteItem (playlist);
			}
		}
	}
}
