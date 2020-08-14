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
        Me.CHName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CHEntree = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CHPath = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CHCommand = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CHOutput = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CHStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ListViewGUI
        '
        Me.ListViewGUI.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.CHName, Me.CHEntree, Me.CHPath, Me.CHCommand, Me.CHOutput, Me.CHStatus})
        Me.ListViewGUI.ForeColor = System.Drawing.Color.MidnightBlue
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
        'CHName
        '
        Me.CHName.Text = "Name"
        Me.CHName.Width = 250
        '
        'CHEntree
        '
        Me.CHEntree.Text = "Input"
        '
        'CHPath
        '
        Me.CHPath.Text = "Path"
        '
        'CHCommand
        '
        Me.CHCommand.Text = "Command"
        Me.CHCommand.Width = 200
        '
        'CHOutput
        '
        Me.CHOutput.Text = "Output"
        '
        'CHStatus
        '
        Me.CHStatus.Text = "Status"
        Me.CHStatus.Width = 90
        '
        'ButtonClose
        '
        Me.ButtonClose.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.ButtonClose.ForeColor = System.Drawing.Color.MidnightBlue
        Me.ButtonClose.Location = New System.Drawing.Point(674, 496)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 36)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = False
        '
        'FormGUImanager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(778, 544)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.ListViewGUI)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "FormGUImanager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GUI Manager"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListViewGUI As ListView
    Friend WithEvents CHName As ColumnHeader
    Friend WithEvents CHEntree As ColumnHeader
    Friend WithEvents CHPath As ColumnHeader
    Friend WithEvents CHCommand As ColumnHeader
    Friend WithEvents CHOutput As ColumnHeader
    Friend WithEvents CHStatus As ColumnHeader
    Friend WithEvents ButtonClose As Button
End Class
