﻿namespace Tetris
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
            components = new System.ComponentModel.Container();
            _renderTimer = new System.Windows.Forms.Timer(components);
            _gameTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // renderTimer
            // 
            _renderTimer.Interval = 250;
            _renderTimer.Tick += renderTimer_Tick;
            // 
            // gameTimer
            // 
            _gameTimer.Interval = 750;
            _gameTimer.Tick += gameTimer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            DoubleBuffered = true;
            Name = "Form1";
            Text = "Form1";
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer _renderTimer;
        private System.Windows.Forms.Timer _gameTimer;
    }
}