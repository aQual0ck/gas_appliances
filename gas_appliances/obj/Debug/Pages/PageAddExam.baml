   M S B A M L   `   `   `  ����I  Egas_appliances, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:gas_appliances.Pagesgas_appliances.Pages  5      &     gas_appliances.Pages.PageAddExam   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:gas_appliances.Pages     5        N� Title$  PageAddExam��5	      .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $	��430 �6   .+�5   
   o� $��0��6   $��Center=�6   .@�5      m� -   5        $��
Назад��5      6&   5      �� 5   
   $	��350��6   $	��400��6    
����������#666666$��1��5      �� 5   
   $��Center=�6   $��Center=�62   $��1��5      .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Прибор:��6   $	��150��6+   �� 5      -   5        $��cmbAppliance��5      $	��150��6/   $��20��6;   $��
5, 5, 0, 5q�6G    �� IsTextSearchEnabled .   �� IsTextSearchCaseSensitive .  5      5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Проверяющий:��6   $	��150��60   �� 5       -   5        $��cmbUser��5       $	��150��6*   $��20��66   $��
5, 5, 0, 5q�6B    .  .  5"      5#      �� 5$      $ 
Horizontal=�6   .+�5%      �� $ ��Дата проверки:��6   $	��150��62   (  "System.Windows.Controls.DatePicker  5&      -   5        $��dpExam��5&      $	��150��6+   $��
5, 5, 0, 5q�67   5'      �� 5(      -   5        $��btnSave��5(      $��Сохранить��6$   $��75��5)       � 	Resources 5*       �� 
TargetType �� Property �� Value��%^   '��       �� 5+      " ��6   .�5,      ��  �@��CornerRadius$ 7��69   5-      5.      5/      50      51      52      