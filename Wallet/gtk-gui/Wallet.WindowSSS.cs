
// This file has been generated by the GUI designer. Do not modify.
namespace Wallet
{
	public partial class WindowSSS
	{
		private global::Gtk.VBox vbox1;
		
		private global::Gtk.HBox hbox4;
		
		private global::Gtk.Image image1;
		
		private global::Wallet.MainMenu testtabsbarwidget1;
		
		private global::Gtk.HBox hbox1;
		
		private global::Wallet.VerticalMenu testtabsbarvertwidget1;
		
		private global::Wallet.MainArea mainarea1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Wallet.WindowSSS
			this.Name = "Wallet.WindowSSS";
			this.Title = global::Mono.Unix.Catalog.GetString ("ZEN Wallet");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.AllowShrink = true;
			// Container child Wallet.WindowSSS.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Homogeneous = true;
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.image1 = new global::Gtk.Image ();
			this.image1.Name = "image1";
			this.image1.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Wallet.Assets.logosmall.png");
			this.hbox4.Add (this.image1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.image1]));
			w1.Position = 1;
			w1.Expand = false;
			w1.Fill = false;
			this.vbox1.Add (this.hbox4);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox4]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			w2.Padding = ((uint)(15));
			// Container child vbox1.Gtk.Box+BoxChild
			this.testtabsbarwidget1 = new global::Wallet.MainMenu ();
			this.testtabsbarwidget1.Events = ((global::Gdk.EventMask)(256));
			this.testtabsbarwidget1.Name = "testtabsbarwidget1";
			this.vbox1.Add (this.testtabsbarwidget1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.testtabsbarwidget1]));
			w3.Position = 1;
			w3.Expand = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			// Container child hbox1.Gtk.Box+BoxChild
			this.testtabsbarvertwidget1 = new global::Wallet.VerticalMenu ();
			this.testtabsbarvertwidget1.Events = ((global::Gdk.EventMask)(256));
			this.testtabsbarvertwidget1.Name = "testtabsbarvertwidget1";
			this.hbox1.Add (this.testtabsbarvertwidget1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.testtabsbarvertwidget1]));
			w4.Position = 0;
			w4.Expand = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.mainarea1 = new global::Wallet.MainArea ();
			this.mainarea1.Events = ((global::Gdk.EventMask)(256));
			this.mainarea1.Name = "mainarea1";
			this.hbox1.Add (this.mainarea1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.mainarea1]));
			w5.Position = 1;
			this.vbox1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
			w6.Position = 2;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 936;
			this.DefaultHeight = 936;
			this.Show ();
			this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		}
	}
}
