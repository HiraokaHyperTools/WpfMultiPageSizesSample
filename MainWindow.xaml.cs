using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMultiPageSizesSample.Models;

namespace WpfMultiPageSizesSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<MyPageSize> list = new List<MyPageSize>();

            list.Add(new MyPageSize { name = "A4", tate = true, });
            list.Add(new MyPageSize { name = "A4", tate = false, });
            list.Add(new MyPageSize { name = "A3", tate = true, });
            list.Add(new MyPageSize { name = "A3", tate = false, });
            list.Add(new MyPageSize { name = "B4", tate = true, });
            list.Add(new MyPageSize { name = "B4", tate = false, });

            page1.ItemsSource = page2.ItemsSource = page3.ItemsSource = list;
            page1.SelectedItem = page2.SelectedItem = page3.SelectedItem = list[0];
        }

        private void printNow_Click(object sender, RoutedEventArgs e)
        {
            printNowInternal();
        }

        void printNowInternal()
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var ticket = dialog.PrintTicket;

            var pageMediaSizes = dialog.PrintQueue.GetPrintCapabilities().PageMediaSizeCapability.ToArray();

            var writer = PrintQueue.CreateXpsDocumentWriter(dialog.PrintQueue);

            var batch = writer.CreateVisualsCollator();

            batch.BeginBatchWrite();

            var adjuster = new PageSizeAndRotationAdjuster(
                ticket,
                pageMediaSizes
                );

            // 1 ページ目
            adjuster.Adjust((MyPageSize)page1.SelectedItem);
            batch.Write(printContents, ticket);

            selectedSize1.Text = ticket.PageMediaSize.PageMediaSizeName + "";

            // 2 ページ目
            adjuster.Adjust((MyPageSize)page2.SelectedItem);
            batch.Write(printContents, ticket);

            selectedSize2.Text = ticket.PageMediaSize.PageMediaSizeName + "";

            // 3 ページ目
            adjuster.Adjust((MyPageSize)page3.SelectedItem);
            batch.Write(printContents, ticket);

            selectedSize3.Text = ticket.PageMediaSize.PageMediaSizeName + "";

            batch.EndBatchWrite();
        }

        class PageSizeAndRotationAdjuster
        {
            private PageMediaSize[] pageMediaSizes;
            private PrintTicket ticket;

            public PageSizeAndRotationAdjuster(PrintTicket ticket, PageMediaSize[] pageMediaSizes)
            {
                this.ticket = ticket;
                this.pageMediaSizes = pageMediaSizes;
            }

            public void Adjust(MyPageSize myPageSize)
            {
                var pageMediaSize = pageMediaSizes
                    // PageMediaSize の選定方法は必要に応じて修正してください。
                    .FirstOrDefault(oneSize => (oneSize.PageMediaSizeName + "").Contains(myPageSize.name));

                if (pageMediaSize != null)
                {
                    ticket.PageMediaSize = pageMediaSize;
                    ticket.PageOrientation = myPageSize.tate ? PageOrientation.Portrait : PageOrientation.Landscape;
                }
            }
        }
    }
}
