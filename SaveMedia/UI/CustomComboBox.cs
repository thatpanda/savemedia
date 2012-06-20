using System;
using System.Drawing;
using System.Windows.Forms;

using Utility;

namespace SaveMedia
{
    public partial class CustomComboBox : ComboBox
    {
        System.Windows.Forms.ImageList mImageList;
        int mIconSize;

        public CustomComboBox( int aItemHeight = 30, int aIconSize = 24 )
            : base()
        {
            mIconSize = aIconSize;

            InitializeComponent();

            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormattingEnabled = true;
            this.ItemHeight = aItemHeight;
        }

        public void Initialize( params System.Object[] aObjects )
        {
            this.Items.Clear();
            if( aObjects.Length != 0 )
            {
                this.Items.AddRange( aObjects );
                BuildImageList();
                this.SelectedIndex = 0;
            }
            if( aObjects.Length <= 1 )
            {
                this.Enabled = false;
            }
        }

        private void BuildImageList()
        {
            mImageList = new ImageList();
            mImageList.ImageSize = new System.Drawing.Size( mIconSize, mIconSize );

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

            if( e.Index == -1 )
            {
                return;
            }

            Brush theBrush = new SolidBrush( e.ForeColor );

            StringFormat theStringFormat = new StringFormat();
            theStringFormat.LineAlignment = StringAlignment.Center;

            Point thePoint = new Point( e.Bounds.Left, e.Bounds.Top + e.Bounds.Height / 2 );

            if( mImageList != null )
            {
                String theExtension;
                if( StringUtils.StringBetween( this.Items[ e.Index ].ToString(), "(*", ")", out theExtension ) )
                {
                    e.Graphics.DrawImage( mImageList.Images[ e.Index ], e.Bounds.Left, e.Bounds.Top + 3 );
                    thePoint.X += mImageList.ImageSize.Width + 6;
                }
            }

            e.Graphics.DrawString( this.Items[ e.Index ].ToString(), e.Font, theBrush, thePoint, theStringFormat );

            e.DrawFocusRectangle();
        }
    }
}
