using System;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.ManualReview)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 1666, "Use WKWebView on iOS", PlatformAffected.iOS)]
	public class Issue1666 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		protected override void Init()
		{
			var buttonBack = new Button() { Text = "<", BackgroundColor = Color.LightBlue, AutomationId = "buttonBack" };
			var buttonNext = new Button() { Text = ">", BackgroundColor = Color.LightBlue, AutomationId = "buttonNext" };
			var buttonClear = new Button() { Text = "CLEAR", BackgroundColor = Color.LightBlue, AutomationId = "buttonClear" };
			var buttonState = new Button() { Text = "STATE", BackgroundColor = Color.LightBlue, AutomationId = "buttonState" };

			var buttonA = new Button() { Text = "CNN", BackgroundColor = Color.LightBlue, AutomationId = "buttonA" };
			var buttonB = new Button() { Text = "HTML", BackgroundColor = Color.LightBlue, AutomationId = "buttonB" };
			var buttonC = new Button() { Text = "C", BackgroundColor = Color.LightBlue, AutomationId = "buttonC" };
			var buttonD = new Button() { Text = "D", BackgroundColor = Color.LightBlue, AutomationId = "buttonD" };

			var html = $"<html><body><a href=\"https://www.cnn.com\">CNN</a></body></html>";

			var webView = new WebView()
			{
				HeightRequest = 40,
				Source = new HtmlWebViewSource { Html = html }
			};

			var vcr = new Grid();
			vcr.Children.AddHorizontal(new[] { buttonBack, buttonNext, buttonClear, buttonState });

			var evals = new Grid();
			evals.Children.AddHorizontal(new[] { buttonA, buttonB, buttonC, buttonD });

			var entry = new Entry() { AutomationId = "entry" };
			entry.BackgroundColor = Color.Wheat;

			var buttons = new Grid();
			buttons.Children.AddVertical(vcr);
			buttons.Children.AddVertical(evals);
			buttons.Children.AddVertical(entry);

			var console = new Label()
			{
				AutomationId = "console",
				Text = "Loaded\n"
			};
			Action<string> log = s => { console.Text = s + "\n" + console.Text; };

			var grid = new Grid();
			grid.Children.AddVertical(webView);
			grid.Children.AddVertical(buttons);
			grid.Children.AddVertical(new ScrollView() { Content = console });

			buttonA.Clicked += (s, e) => 
			{
				webView.Source = new UrlWebViewSource() { Url = "https://www.cnn.com" };
			};

			buttonB.Clicked += (s, e) => {
				webView.Source = new HtmlWebViewSource()
				{
					Html = html
				};
			};

			buttonNext.Clicked += (s, e) => { webView.GoForward(); log($"GoForward: {webView.CanGoBack}/{webView.CanGoForward}"); };
			buttonBack.Clicked += (s, e) => { webView.GoBack(); log($"GoBack: {webView.CanGoBack}/{webView.CanGoForward}"); };
			buttonClear.Clicked += (s, e) => { console.Text = ""; };
			buttonState.Clicked += (s, e) => {
				log($"F/B: {webView.CanGoBack}/{webView.CanGoForward}");
				log($"Source: {webView.Source.ToString()}");
			};

			webView.Navigating += (s, e) =>
			{
				var text = $"Navigating {e.NavigationEvent}, ";
				text += $"Cancel: {e.Cancel};";
				entry.Text = e.Url;
				//text += $"Source: {e.Source}\n";
				log(text);
			};

			webView.Navigated += (s, e) =>
			{
				var text = $"Navigated {e.NavigationEvent}, ";
				text += $"Result: {e.Result}";
				entry.Text = e.Url;
				//text += $"Source: {e.Source}\n";
				log(text);
			};

			// Initialize ui here instead of ctor
			Content = grid;
			BackgroundColor = Color.Gray;
		}

		private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
		{
			throw new System.NotImplementedException();
		}

#if UITEST
		[Test]
		public void Issue1Test() 
		{
			// Delete this and all other UITEST sections if there is no way to automate the test. Otherwise, be sure to rename the test and update the Category attribute on the class. Note that you can add multiple categories.
			RunningApp.Screenshot ("I am at Issue 1");
			RunningApp.WaitForElement (q => q.Marked ("IssuePageLabel"));
			RunningApp.Screenshot ("I see the Label");
		}
#endif
	}
}