﻿using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using FAB.Forms;
using Acr.UserDialogs;

namespace Capp2
{
	public static class TutorialHelper
	{
		static Label InfoLabel, DoneLabel, AutoCallInfoLabel, InfoLabel2, InfoLabel3;
		static StackLayout stack;
		static FloatingActionButton fab;
		static bool continuePressed = false;
		static double continuePositionY;
		static double continuePositionX;
		static Image img;
		public static bool WelcomeTipDone = false, AutoCallTipDone = false, BackToPlaylistPageDone = false, HowToMakeNamelistDone = false, 
			HowToAddContactsDone = false, ReadyForAutoCallTipDone = false, ReadyForExtraTips = false, ExtraTipsDone = false;
		public static ContentPage PrevPage;
		public static RelativeLayout layout = new RelativeLayout();
		public static ContentView content = new ContentView();
		public static double BackgroundColorAlpha{get; set;} = 0.9;

		public static RelativeLayout GetRelativeLayoutWrapper(){
			return layout;
		}

		public static async Task<RelativeLayout> ShowExtraTips(ContentPage page, Color background, 
			string text = "A few extra tips...", bool tipshowintutorial = true)
		{
			Debug.WriteLine ("In ShowExtraTips");

			layout = ((RelativeLayout)page.Content);

			Debug.WriteLine ("Assigned layout");

			InfoLabel = UIBuilder.CreateTutorialLabel (text, NamedSize.Large, FontAttributes.None);

			Debug.WriteLine ("Created Tutorial label");

			stack = new StackLayout { 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					BackgroundColorAlpha
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							UIBuilder.CreateEmptyStackSpace(),
							InfoLabel,
							UIBuilder.CreateEmptyStackSpace(),
							UIBuilder.CreateTutorialVideoPickerView(new VideoChooserItem[]{
								new VideoChooserItem{
									ImagePath = "HowToCappScreenshot.png",
									LabelText = "Where'd my CAPP Sheet go?",
									VideoPath = "HowToCapp.mov"
								},
								new VideoChooserItem{
									ImagePath = "HowToUseTextTemplatesScreenshot.png",
									LabelText = "Type your meetup texts once, then never again",
									VideoPath = "HowToUseTextTemplates.mov"
								},
								new VideoChooserItem{
									ImagePath = "HowToUseStatsScreenshot.png",
									LabelText = "See your daily work stats!",
									VideoPath = "HowToUseStats.mov"
								},
								new VideoChooserItem{
									ImagePath = "HowToFeedbackScreenshot.png",
									LabelText = "Suggestions, Feedback, Questions?",
									VideoPath = "HowToFeedback.mov"
								},
								new VideoChooserItem{
									ImagePath = "HowToSendYesCallsScreenshot.png",
									LabelText = "Send your daily yes calls from CapTap!",
									VideoPath = "HowToUseSendYesCalls.mov"
								},
								new VideoChooserItem{
									ImagePath = "HowToSetStartingScreenshot.png",
									LabelText = "Always start with this namelist",
									VideoPath = "HowToSetStarting.mov"
								},
								new VideoChooserItem{
									ImagePath = "HowToUseDailyEmailScreenshot.png",
									LabelText = "Daily Emails",
									VideoPath = "HowToUseDailyEmail.mov"
								},


							})
						}
					}
				}
			};

			Debug.WriteLine ("stacklayout done");

			content = new ContentView{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = stack
			};

			Debug.WriteLine ("contentview initialized");

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			Debug.WriteLine ("content added to layout");

			if (tipshowintutorial) {
				ResetContinueLabel (layout, new Command (async () => {
					App.InTutorialMode = false;
					layout.Children.Remove (content.Content);
					await AlertHelper.Alert ("That's pretty much it!", 
						"If you forget, you can replay this tutorial in the Settings page at anytime. We'll return you to the main namelist now :)");
					UserDialogs.Instance.ShowLoading ();
					App.NavPage = new NavigationPage (new PlaylistPage ());
					App.MasterDetailPage.Detail = App.NavPage;
					App.NavPage.Navigation.PushAsync (new CAPP (Values.ALLPLAYLISTPARAM));
					UserDialogs.Instance.HideLoading ();
				}), true, true);
			} else {
				Debug.WriteLine ("not shown in tutorial mode");

				DoneLabel = UIBuilder.CreateTutorialLabel ("Got it", NamedSize.Large, FontAttributes.Bold, 
					LineBreakMode.WordWrap, new Command(()=>{
						App.NavPage.Navigation.PopModalAsync();
					}));
				UIBuilder.AddElementRelativeToViewonRelativeLayoutParent(layout, DoneLabel,
					Constraint.RelativeToParent((parent) =>  { return parent.Width *0.7; }),
					Constraint.RelativeToParent((parent) =>  { return parent.Height * 0.92 ; })
				);
				UIAnimationHelper.StartPressMeEffectOnView (DoneLabel);
			}

			Debug.WriteLine ("donelabel reset");

			return layout;
		}

		public static async Task ReadyForAutoCallTip(CAPP page, Color background,
			string text = "We're all set!\n\nNow try pressing the AutoCall icon")
		{
			layout = ((RelativeLayout)page.Content);

			InfoLabel = UIBuilder.CreateTutorialLabel (text, NamedSize.Large, FontAttributes.None);

			img = UIBuilder.CreateTappableImage ("DownFromUpperLeft.png", LayoutOptions.Center, Aspect.AspectFit, 
				new Command (() => {
				}), InfoLabel.FontSize * 0.67);

			stack = new StackLayout { 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					BackgroundColorAlpha
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							UIBuilder.CreateEmptyStackSpace(),
							InfoLabel,
							new StackLayout{
								HorizontalOptions = LayoutOptions.End,
								VerticalOptions = LayoutOptions.End,
								Children = { img }
							}
						}
					}
				}
			};

			content = new ContentView{
				Content = stack
			};

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			page.Content = UIBuilder.AddFABToViewWrapRelativeLayout(layout, fab, new Command(async () => {
				layout.Children.Remove(content.Content);
				layout.Children.Remove(fab);
				ReadyForAutoCallTipDone = true;


				page.AutoCall();
			}));

			while (true) {
				await UIAnimationHelper.ZoomUnZoomElement (img, 1.3, 1000, true);
				await Task.Delay (1000);
			}
		}

		public static async Task OpenNamelist(PlaylistPage page, string text, Color background){
			layout = ((RelativeLayout)page.Content);
			continuePositionX = DoneLabel.X;
			continuePositionY = DoneLabel.Y;
			DoneLabel.Opacity = 0;

			InfoLabel = UIBuilder.CreateTutorialLabel (text, NamedSize.Large, FontAttributes.None);

			stack = new StackLayout { 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					0.9
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							UIBuilder.CreateEmptyStackSpace(),
							InfoLabel,
							UIBuilder.CreateEmptyStackSpace(),
							DoneLabel
						}
					}
				}
			};

			content = new ContentView{
				Content = stack
			};

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			DoneLabel = UIBuilder.CreateTutorialLabel ("Got it", NamedSize.Large, FontAttributes.Bold, 
				LineBreakMode.WordWrap, new Command(()=>{
					TutorialHelper.PrevPage = page;
					layout.Children.Remove(content.Content);
					//await Task.Delay(500);
					App.NavPage = new NavigationPage(new PlaylistPage());
					App.MasterDetailPage.Detail = App.NavPage;
				}));
			UIBuilder.AddElementRelativeToViewonRelativeLayoutParent(layout, DoneLabel,
				Constraint.RelativeToParent((parent) =>  { return (continuePositionX); }),
				Constraint.RelativeToParent((parent) =>  { return (continuePositionY) ; })
			);
			DoneLabel.Opacity = 1;
		}

		public static bool PrevPageIsPlaylistPage(){
			return (TutorialHelper.PrevPage.GetType() == new PlaylistPage().GetType());
		}

		public static async Task HowToAddNumbers(ContentPage page, string text, Color background){
			layout = ((RelativeLayout)page.Content);

			InfoLabel = UIBuilder.CreateTutorialLabel (text, NamedSize.Large, FontAttributes.None);

			img = UIBuilder.CreateTappableImage ("UpFromBottomLeft.png", LayoutOptions.Center, Aspect.AspectFit, new Command (() => {
			}), InfoLabel.FontSize);

			stack = new StackLayout { 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					0.9
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							UIBuilder.CreateEmptyStackSpace(),
							new StackLayout{
								Orientation = StackOrientation.Horizontal,
								Children = {
									InfoLabel,
									new StackLayout{
										HorizontalOptions = LayoutOptions.End,
										Children = { img }
									}
								}
							}
						}
					}
				}
			};

			content = new ContentView{
				Content = stack
			};

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			while (true) {
				await UIAnimationHelper.ZoomUnZoomElement (img, 1.3, 1000, true);
				await Task.Delay (1000);
			}
		}

		public static async Task HowToMakeANamelist(PlaylistPage page, string text, Color background){
			Debug.WriteLine ("In HowToMakeANamelist");

			layout = ((RelativeLayout)page.Content);

			InfoLabel = UIBuilder.CreateTutorialLabel (text, NamedSize.Large, FontAttributes.None);

			Debug.WriteLine ("InfoLabel assigned");

			img = UIBuilder.CreateTappableImage ("DownFromUpperLeft.png", LayoutOptions.Center, Aspect.AspectFit, new Command (() => {
			}), InfoLabel.FontSize);

			Debug.WriteLine ("Created arrow image");

			stack = new StackLayout { 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					0.9
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							UIBuilder.CreateEmptyStackSpace(),
							new StackLayout{
								Orientation = StackOrientation.Horizontal,
								Children = {
									InfoLabel,
									new StackLayout{
										HorizontalOptions = LayoutOptions.End,
										VerticalOptions = LayoutOptions.End,
										Children = { img }
									}
								}
							}
						}
					}
				}
			};

			Debug.WriteLine ("stack layout assigned");

			content = new ContentView{
				Content = stack
			};

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			Debug.WriteLine ("page content added to relativelayout");

			fab = UIBuilder.CreateFAB ("", FabSize.Normal, Color.FromHex (Values.GOOGLEBLUE), 
				Color.FromHex (Values.PURPLE));

			Debug.WriteLine ("Created fab");

			page.Content = UIBuilder.AddFABToViewWrapRelativeLayout(layout, fab, new Command(async () => {
				layout.Children.Remove(content.Content);
				layout.Children.Remove(fab);
				TutorialHelper.HowToMakeNamelistDone = true;
				await Util.AddNamelist(page);
			}));

			Debug.WriteLine ("added fab");

			while (true) {
				await UIAnimationHelper.ZoomUnZoomElement (img, 1.3, 1000, true);
				await Task.Delay (1000);
			}
		}
		public static async Task ShowTip_HowToGoBackToPlaylistPage(ContentPage page, string text, Color background){
			layout = ((RelativeLayout)page.Content);

			InfoLabel = UIBuilder.CreateTutorialLabel (text, NamedSize.Large, FontAttributes.None);

			img = UIBuilder.CreateTappableImage ("UpArrow.png", LayoutOptions.Center, Aspect.AspectFit, new Command (() => {
			}), InfoLabel.FontSize);

			stack = new StackLayout { 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					0.9
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							UIBuilder.CreateEmptyStackSpace(),
							new StackLayout{
								Orientation = StackOrientation.Horizontal,
								Children = {
									img, InfoLabel
								}
							}
						}
					}
				}
			};

			content = new ContentView{
				Content = stack
			};

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			while (true) {
				await UIAnimationHelper.ZoomUnZoomElement (img, 1.3, 1000, true);
				await Task.Delay (1000);
			}
		}

		public static async Task ShowTip_Welcome(ContentPage page, string TutorialText, Color background){
			layout = ((RelativeLayout)page.Content);

			Debug.WriteLine ("assigned layout");

			fab = UIBuilder.CreateFAB ("", FabSize.Normal, Color.FromHex (Values.GOOGLEBLUE), 
				Color.FromHex (Values.GOOGLEBLUE));

			Debug.WriteLine ("created fab");

			AutoCallInfoLabel = UIBuilder.CreateTutorialLabel (
				"Introducing AUTOCALL\n\nDouble your daily yes calls.\n\nNo more dialling. " +
				"No more typing every text. \nNo more papers to lose.\nNo losing track of your sched\n\n" +
				"CapTap does it all for you",
				NamedSize.Small, FontAttributes.Bold, LineBreakMode.WordWrap,
				new Command(() => {
					UIAnimationHelper.ShrinkUnshrinkElement(AutoCallInfoLabel);
				})
			);

			Debug.WriteLine ("created autocallinfolabel");

			DoneLabel = UIBuilder.CreateTutorialLabel ("Continue", NamedSize.Large, FontAttributes.Bold,
				LineBreakMode.WordWrap, new Command (async () => {
					WelcomeTipDone = true;
					await ShowAutoCallTip (page, layout, fab);
				}));

			InfoLabel = UIBuilder.CreateLabel (NamedSize.Large);
			InfoLabel.LineBreakMode = LineBreakMode.WordWrap;
			InfoLabel.Text = TutorialText;
			InfoLabel.FontAttributes = FontAttributes.Bold;

			InfoLabel2 = UIBuilder.CreateTutorialLabel ("The fastest way to burn through your namelists!", 
				NamedSize.Medium);
			InfoLabel3 = UIBuilder.CreateTutorialLabel ("Let's press continue to see how it works!", 
				NamedSize.Medium);

			InfoLabel.TextColor = Color.White;

			stack = new StackLayout{ 
				BackgroundColor = new Color (
					background.R, 
					background.G, 
					background.B, 
					0.9
				),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.Center,
						Padding = new Thickness(60),
						Children = {
							InfoLabel,
							UIBuilder.CreateEmptyStackSpace(),

							InfoLabel2, 
							UIBuilder.CreateEmptyStackSpace(),

							InfoLabel3, 
							UIBuilder.CreateEmptyStackSpace(),
							UIBuilder.CreateEmptyStackSpace(),

							DoneLabel
						}
					}
				}
			};
			Debug.WriteLine ("created stack, assinging to contentview");

			content = new ContentView{
				Content = stack
			};
			Debug.WriteLine ("assigned to contentview, adding tip overlay");

			layout.Children.Add(
				content.Content,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent ((parent) => {
					return parent.Width;
				}),
				Constraint.RelativeToParent((parent) => {
					return parent.Height;
				})
			);

			App.NavPage.Navigation.PushModalAsync (page);
		} 

		public static async Task RemoveHowToAddContactsTipIfNeeded(CAPP capp){
			Debug.WriteLine("About to remove tip and call AddContacts()");
			if (App.InTutorialMode && TutorialHelper.PrevPageIsPlaylistPage () && TutorialHelper.HowToMakeNamelistDone) { 
				Debug.WriteLine("Condition to remove HowToAddContactsTip satisfied");
				try {
					TutorialHelper.layout.Children.Remove (TutorialHelper.content.Content);
					TutorialHelper.PrevPage = capp;
					TutorialHelper.HowToAddContactsDone = true;
				} catch (Exception e) {
					Debug.WriteLine ("Couldn't remove HowToAddContactsTip: {0}", e.Message);
				}
			} else {
				Debug.WriteLine ("Condition to remove HowToAddContactsTip not satisfied: " +
					"In tutorial: {0}, Came from namelist page: {1}, Finished HowToMakeNamelistTip: {2}", App.InTutorialMode,
					TutorialHelper.PrevPageIsPlaylistPage(), TutorialHelper.HowToMakeNamelistDone);
			}
		}

		public static async Task ContinueCAPPTutorialIfNotDone(CAPP capp){
			try{
				if (App.InTutorialMode && TutorialHelper.PrevPageIsPlaylistPage()) {
					
					TutorialHelper.HowToAddNumbers (capp, "Now let's add some contacts so we can try out AutoCall\n" +
						"Just tap '+' up there", 
						Color.FromHex (Values.CAPPTUTORIALCOLOR_Orange));
				} else {
					Debug.WriteLine ("in tutorial, onAppearing CAPP. PrevPage is not playlistpage, " +
						"HowToAddNumbers not activated");
				}
			}catch(NullReferenceException){
				Debug.WriteLine ("PrevPage still null");
			}
		}

		public static async Task ShowAutoCallTip(ContentPage page, RelativeLayout layout, FloatingActionButton fab){
			AutoCallInfoLabel.Opacity = 0;
			continuePositionX = DoneLabel.X;
			continuePositionY = DoneLabel.Y;

			page.Content = UIBuilder.AddFABToViewWrapRelativeLayout(layout, fab, new Command(() => {}));
			layout = ((RelativeLayout)page.Content);

			UIBuilder.AddElementRelativeToViewonRelativeLayoutParent(layout, AutoCallInfoLabel,
				Constraint.RelativeToParent((parent) =>  { return (parent.Width - fab.Width) * 0.15; }),
				Constraint.RelativeToParent((parent) =>  { return (parent.Height * 0.67) ; })
			);

			await Task.Delay(100);
			DoneLabel.Opacity = 0;
			InfoLabel2.FadeTo(0, 500);
			InfoLabel3.FadeTo (0, 500);

			UIAnimationHelper.FlyFromLeft(AutoCallInfoLabel, 1000, true);
			await AutoCallInfoLabel.FadeTo(1, 125, Easing.CubicInOut);
			await UIAnimationHelper.ZoomUnZoomElement(fab, 1.3, 500);

			ResetContinueLabel (layout, new Command (() => {
				PrevPage = page;
				TransitionWelcomeToPlaylistPageTip(layout);
			}));
		}

		public static async Task ResetContinueLabel(RelativeLayout layout, Command ContinueCommand, 
			bool tipshownintutorial = true, bool SetDoneLowerRight = false){
			continuePressed = false;
			if(tipshownintutorial)
				layout.Children.Remove (DoneLabel);

			if (!tipshownintutorial) {
				DoneLabel = UIBuilder.CreateTutorialLabel ("Got it", NamedSize.Large, FontAttributes.Bold, 
					LineBreakMode.WordWrap, new Command (() => {
					ContinueCommand.Execute (null);
				}));
			} else {
				DoneLabel.GestureRecognizers.Clear ();
				DoneLabel.GestureRecognizers.Add (new TapGestureRecognizer{Command = new Command(()=>{
					continuePressed = true;
					ContinueCommand.Execute(null);
				})});
			}

			if (SetDoneLowerRight) {
				UIBuilder.AddElementRelativeToViewonRelativeLayoutParent (layout, DoneLabel,
					Constraint.RelativeToParent ((parent) => {
						return parent.Width * 0.7;
					}),
					Constraint.RelativeToParent ((parent) => {
						return parent.Height * 0.92;
					})
				);
			} else {
				UIBuilder.AddElementRelativeToViewonRelativeLayoutParent(layout, DoneLabel,
					Constraint.RelativeToParent((parent) =>  { return (continuePositionX); }),
					Constraint.RelativeToParent((parent) =>  { return (continuePositionY) ; })
				);
			}

			DoneLabel.FadeTo (1);

			/*if (tipshownintutorial) {
				
			}*/
			while (!continuePressed) {
				await UIAnimationHelper.ZoomUnZoomElement (DoneLabel, 1.1, 1000, true);
				await Task.Delay (1000);
			}
		}

		public static async Task TransitionWelcomeToPlaylistPageTip(RelativeLayout layout){
			AutoCallInfoLabel.FadeTo (0, 800, Easing.SinOut);
			InfoLabel2.FadeTo (0);
			InfoLabel3.FadeTo (0);
			fab.FadeTo(0, 800, Easing.SinOut);
			UIAnimationHelper.SwitchLabelText (InfoLabel, "Let's try it out!", 1000);

			ResetContinueLabel (layout, new Command (() => {
				MessagingCenter.Send("", Values.DONEWAUTOCALLTIP);
				App.NavPage.Navigation.PopModalAsync();
			}));
		}
	}
}

