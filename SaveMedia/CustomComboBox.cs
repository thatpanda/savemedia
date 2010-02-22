using System;
using System.Drawing;
using System.Windows.Forms;

using Utility;

namespace SaveMedia
{
    public partial class CustomComboBox : ComboBox
    {
        System.Windows.Forms.ImageList mImageList;

        public CustomComboBox()
        {
            InitializeComponent();
        }

        public void BuildImageList()
        {
            this.ItemHeight = 30;

            mImageList = new ImageList();
            mImageList.ImageSize = new System.Drawing.Size( 24, 24 );

            foreach( System.Object theItem in this.Items )
            {
                String theExtension;
                Icon theIcon;

                if( StringUtils.StringBetween( theItem.ToString(), "(*", ")", out theExtension ) )
                {
                    theIcon = ImageUtils.AssociatedIcon( theExtension );
                }
                else
                {
                    theIcon = ImageUtils.AssociatedIcon( ".tmp" );
                }

                mImageList.Images.Add( theIcon );
            }
        }

        protected override void OnDrawItem( DrawItemEventArgs e )
        {
            base.OnDrawItem( e );
            e.DrawBackground();

            Brush theBrush = new SolidBrush( e.ForeColor );

            StringFormat theStringFormat = new StringFormat();
            theStringFormat.LineAlignment = StringAlignment.Center;

            Point thePoint = new Point( e.Bounds.Left, e.Bounds.Top + e.Bounds.Height / 2 );

            String theExtension;
            if( StringUtils.StringBetween( this.Items[ e.Index ].ToString(), "(*", ")", out theExtension ) )
            {
                e.Graphics.DrawImage( mImageList.Images[ e.Index ], e.Bounds.Left, e.Bounds.Top + 3 );
                thePoint.X += mImageList.ImageSize.Width + 6;
            }

            e.Graphics.DrawString( this.Items[ e.Index ].ToString(), e.Font, theBrush, thePoint, theStringFormat );

            e.DrawFocusRectangle();
        }
    }
}
