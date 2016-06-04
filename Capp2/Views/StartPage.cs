﻿using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Capp2.Helpers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Capp2
{
	public class StartPage:MasterDetailPage
	{
		StackLayout StatsSettings = new StackLayout(), TemplateSettings = new StackLayout(), 
		FeedbackSettings = new StackLayout(), DefaultNamelistSettings = new StackLayout(), 
		DailyEmailSettings = new StackLayout(), SendYesCallSettings = new StackLayout(), HowToSettings = new StackLayout(),
		SettingsLayout = new StackLayout();

		public StartPage ()
		{
			App.MasterDetailPage = this;
			this.Title = "Namelists";

			Device.OnPlatform(
				() => App.NavPage = new NavigationPage(new PlaylistPage{StartColor = App.StartColor, 
					EndColor = App.EndColor}){
					//BarBackgroundColor = Color.White,
					//BarTextColor = Color.FromHex(Values.GOOGLEBLUE),
				}, 
				() => App.NavPage = new NavigationPage(new PlaylistPage{StartColor = App.StartColor, EndColor = App.EndColor}){
					BarBackgroundColor = Color.FromHex (Values.GOOGLEBLUE),
					BarTextColor = Color.White,
				}
			);

			/*App.NavPage = new NavigationPage (new PlaylistPage{ StartColor = App.StartColor, EndColor = App.EndColor }) {
				BarBackgroundColor = Color.FromHex (Values.GOOGLEBLUE),
				BarTextColor = Color.White,
			};*/

			App.NavPage.Navigation.PopAsync ();
			App.NavPage.Navigation.PushAsync (new CAPP (App.DefaultNamelist));
			this.Detail = App.NavPage;
			this.Master = new ContentPage { 
				Icon = "Hamburger-blue.png",
				Title = "Settings", 
				BackgroundColor = Color.FromHex ("#333333"),
				Content = createUI (),
			};

			AdHelper.AddGreenURLOrangeTitleBannerToStack (SettingsLayout);
		}

		ScrollView createUI(){
			CreateSettings ();

			SettingsLayout = new StackLayout {
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness (20),
				Children = { 
					UIBuilder.CreateSettingsHeader ("Settings"),

					StatsSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

					TemplateSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

					FeedbackSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

					DefaultNamelistSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

					DailyEmailSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

					SendYesCallSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

					HowToSettings,
					UIBuilder.CreateSeparator (Color.Gray, 0.3),

				},
			};

			return new ScrollView {
				Content = SettingsLayout
			};
		}
		void CreateSettings(){
			HowToSettings = UIBuilder.CreateSetting ("help.png", "\tHelp", 
				new TapGestureRecognizer{ Command = new Command(() => {
					UIAnimationHelper.ZoomUnZoomElement(HowToSettings);
					App.NavPage.Navigation.PushModalAsync(new TutorialSettingsPage());
					/*App.NavPage.Navigation.PushModalAsync(new CarouselHelper(IndicatorStyleEnum.Dots, 
						Color.FromHex(Values.CAPPTUTORIALCOLOR_Orange)));*/
			})});
			StatsSettings = UIBuilder.CreateSetting ("trending-Medium.png", "\tMy CAP Stats", 
				new TapGestureRecognizer{Command = new Command(() =>
					{
						UIAnimationHelper.ZoomUnZoomElement(StatsSettings); 
						App.NavPage.Navigation.PushModalAsync(new CapStats());
					}
				)});
			SendYesCallSettings = UIBuilder.CreateSetting ("Leaderboard.png", "\tSend Yes Calls Today", 
				new TapGestureRecognizer{Command = new Command(() =>
					{
						UIAnimationHelper.ZoomUnZoomElement(SendYesCallSettings);
						/*UIAnimationHelper.ShrinkUnshrinkElement(StatsSettings); 
						UIAnimationHelper.ShrinkUnshrinkElement(TemplateSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(FeedbackSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(DefaultNamelistSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(DailyEmailSettings); 
*/
						DependencyService.Get<IPhoneContacts>().Share(
							StatsHelper.GetYesCallMessage()
						);
					}
				)});
			DailyEmailSettings = UIBuilder.CreateSetting ("Message-100-yellow.png", "\tDaily Emails", 
				new TapGestureRecognizer{Command = new Command(() =>
					{
						UIAnimationHelper.ZoomUnZoomElement(DailyEmailSettings); 
						/*UIAnimationHelper.ShrinkUnshrinkElement(StatsSettings); 
						UIAnimationHelper.ShrinkUnshrinkElement(TemplateSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(FeedbackSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(DefaultNamelistSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(SendYesCallSettings);
*/
						DependencyService.Get<IEmailService>().SendEmail(Settings.DailyEmailTemplateSettings);
					}
				)});
			DefaultNamelistSettings = UIBuilder.CreateSetting ("FinishFlag.png", "\tStarting Namelist", 
				new TapGestureRecognizer{Command = new Command(async () =>
					{
						UIAnimationHelper.ZoomUnZoomElement(DefaultNamelistSettings); 
						/*UIAnimationHelper.ShrinkUnshrinkElement(TemplateSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(FeedbackSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(StatsSettings);
						UIAnimationHelper.ShrinkUnshrinkElement(DailyEmailSettings); 
						UIAnimationHelper.ShrinkUnshrinkElement(SendYesCallSettings);
*/
						await Util.ChooseNewDefaultNamelist(App.Database.GetPlaylistNames());
					}
				)});
			TemplateSettings = UIBuilder.CreateSetting ("SpeechBubble.png", "\tText Templates", 
				new TapGestureRecognizer {Command = new Command (() => {
					UIAnimationHelper.ZoomUnZoomElement(TemplateSettings);
					/*UIAnimationHelper.ShrinkUnshrinkElement(StatsSettings); 
					UIAnimationHelper.ShrinkUnshrinkElement(FeedbackSettings);
					UIAnimationHelper.ShrinkUnshrinkElement(DefaultNamelistSettings);
					UIAnimationHelper.ShrinkUnshrinkElement(DailyEmailSettings); 
					UIAnimationHelper.ShrinkUnshrinkElement(SendYesCallSettings);
*/
					App.NavPage.Navigation.PushModalAsync (new TemplateSettingsPage ());
				}
				)});
			FeedbackSettings = UIBuilder.CreateSetting ("Feedback.png", "\tContact Team CapTap", 
				new TapGestureRecognizer {Command = new Command (() => {
					UIAnimationHelper.ZoomUnZoomElement(FeedbackSettings);
					/*UIAnimationHelper.ShrinkUnshrinkElement(StatsSettings); 
					UIAnimationHelper.ShrinkUnshrinkElement(TemplateSettings);
					UIAnimationHelper.ShrinkUnshrinkElement(DefaultNamelistSettings);
					UIAnimationHelper.ShrinkUnshrinkElement(DailyEmailSettings); 
					UIAnimationHelper.ShrinkUnshrinkElement(SendYesCallSettings);
*/
					if (Device.OS == TargetPlatform.Android){
						//Device.OpenUri (new Uri ("mailto:captapuserfeedback@gmail.com"));
					}
					else if (Device.OS == TargetPlatform.iOS) {
						DependencyService.Get<IEmailService> ().SendEmail ();
					}})});
		}
	}
}

