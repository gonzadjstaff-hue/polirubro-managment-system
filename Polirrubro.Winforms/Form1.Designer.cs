namespace Polirrubro.Winforms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            btnNuevoArticulo = new Button();
            btnEliminarArticulo = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(42, 29);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(706, 344);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btnNuevoArticulo
            // 
            btnNuevoArticulo.Location = new Point(636, 400);
            btnNuevoArticulo.Name = "btnNuevoArticulo";
            btnNuevoArticulo.Size = new Size(104, 28);
            btnNuevoArticulo.TabIndex = 1;
            btnNuevoArticulo.Text = "Nuevo Artículo";
            btnNuevoArticulo.UseVisualStyleBackColor = true;
            btnNuevoArticulo.Click += btnNuevoArticulo_Click;
            // 
            // btnEliminarArticulo
            // 
            btnEliminarArticulo.Location = new Point(488, 400);
            btnEliminarArticulo.Name = "btnEliminarArticulo";
            btnEliminarArticulo.Size = new Size(108, 28);
            btnEliminarArticulo.TabIndex = 2;
            btnEliminarArticulo.Text = "Eliminar Artículo";
            btnEliminarArticulo.UseVisualStyleBackColor = true;
            btnEliminarArticulo.Click += btnEliminarArticulo_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnEliminarArticulo);
            Controls.Add(btnNuevoArticulo);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Button btnNuevoArticulo;
        private Button btnEliminarArticulo;
    }
}
