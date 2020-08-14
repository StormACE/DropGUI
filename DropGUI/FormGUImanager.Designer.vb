<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormGUImanager
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormGUImanager))
        Me.ListViewGUI = New System.Windows.Forms.ListView()
        Me.SuspendLayout()
        '
        'ListViewGUI
        '
        Me.ListViewGUI.FullRowSelect = True
        Me.ListViewGUI.GridLines = True
        Me.ListViewGUI.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListViewGUI.HideSelection = False
        Me.ListViewGUI.Location = New System.Drawing.Point(30, 30)
        Me.ListViewGUI.MultiSelect = False
        Me.ListViewGUI.Name = "ListViewGUI"
        Me.ListViewGUI.Size = New System.Drawing.Size(719, 379)
        Me.ListViewGUI.TabIndex = 0
        Me.ListViewGUI.UseCompatibleStateImageBehavior = False
        Me.ListViewGUI.View = System.Windows.Forms.View.Details
        '
        'FormGUImanager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(778, 544)
        Me.Controls.Add(Me.ListViewGUI)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FormGUImanager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GUI Manager"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListViewGUI As ListView
End Class
