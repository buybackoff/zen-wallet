
// This file has been generated by the GUI designer. Do not modify.
namespace Wallet.Widgets
{
	public partial class Status
	{
		private global::Gtk.EventBox eventbox1;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Label label1;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Wallet.Widgets.Status
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Wallet.Widgets.Status";
			// Container child Wallet.Widgets.Status.Gtk.Container+ContainerChild
			this.eventbox1 = new global::Gtk.EventBox();
			this.eventbox1.Name = "eventbox1";
			// Container child eventbox1.Gtk.Container+ContainerChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			this.hbox3.BorderWidth = ((uint)(15));
			// Container child hbox3.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.hbox3.Add(this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			this.eventbox1.Add(this.hbox3);
			this.Add(this.eventbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}
