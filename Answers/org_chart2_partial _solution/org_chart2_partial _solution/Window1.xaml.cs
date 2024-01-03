using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace org_chart2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Build the tree.
            NaryNode<string> generi_gloop = BuildGeneriGloopTree();

            // Draw the tree.
            generi_gloop.ArrangeAndDrawSubtree(mainCanvas, 10, 10);
        }

        // Build a test tree.
        private NaryNode<string> BuildGeneriGloopTree()
        {
            // Build the top levels.
            NaryNode<string> generi_gloop = new NaryNode<string>("GeneriGloop");
            NaryNode<string> r_d = new NaryNode<string>("R & D");
            NaryNode<string> sales = new NaryNode<string>("Sales");
            NaryNode<string> professional_services = new NaryNode<string>("Professional\nServices");
            NaryNode<string> applied = new NaryNode<string>("Applied");
            NaryNode<string> basic = new NaryNode<string>("Basic");
            NaryNode<string> advanced = new NaryNode<string>("Advanced");
            NaryNode<string> sci_fi = new NaryNode<string>("Sci Fi");
            NaryNode<string> inside_sales = new NaryNode<string>("Inside\nSales");
            NaryNode<string> outside_sales = new NaryNode<string>("Outside\nSales");
            NaryNode<string> b2b = new NaryNode<string>("B2B");
            NaryNode<string> consumer = new NaryNode<string>("Consumer");
            NaryNode<string> account_management = new NaryNode<string>("Account\nManagement");
            NaryNode<string> hr = new NaryNode<string>("HR");
            NaryNode<string> accounting = new NaryNode<string>("Accounting");
            NaryNode<string> legal = new NaryNode<string>("Legal");

            generi_gloop.AddChild(r_d);
            generi_gloop.AddChild(sales);
            generi_gloop.AddChild(professional_services);

            professional_services.AddChild(hr);
            professional_services.AddChild(accounting);
            professional_services.AddChild(legal);

            // Build the bottom levels.
            // Change to 'if (true)' to build the whole tree.
            if (true)
            {
                NaryNode<string> training = new NaryNode<string>("Training");
                NaryNode<string> hiring = new NaryNode<string>("Hiring");
                NaryNode<string> equity = new NaryNode<string>("Equity");
                NaryNode<string> discipline = new NaryNode<string>("Discipline");
                NaryNode<string> payroll = new NaryNode<string>("Payroll");
                NaryNode<string> billing = new NaryNode<string>("Billing");
                NaryNode<string> reporting = new NaryNode<string>("Reporting");
                NaryNode<string> opacity = new NaryNode<string>("Opacity");
                NaryNode<string> compliance = new NaryNode<string>("Compliance");
                NaryNode<string> progress_prevention = new NaryNode<string>("Progress\nPrevention");
                NaryNode<string> bail_services = new NaryNode<string>("Bail\nServices");

                r_d.AddChild(applied);
                r_d.AddChild(basic);
                r_d.AddChild(advanced);
                r_d.AddChild(sci_fi);

                sales.AddChild(inside_sales);
                sales.AddChild(outside_sales);
                sales.AddChild(b2b);
                sales.AddChild(consumer);
                sales.AddChild(account_management);

                hr.AddChild(training);
                hr.AddChild(hiring);
                hr.AddChild(equity);
                hr.AddChild(discipline);

                accounting.AddChild(payroll);
                accounting.AddChild(billing);
                accounting.AddChild(reporting);
                accounting.AddChild(opacity);

                legal.AddChild(compliance);
                legal.AddChild(progress_prevention);
                legal.AddChild(bail_services);
            }
            return generi_gloop;
        }
    }
}
