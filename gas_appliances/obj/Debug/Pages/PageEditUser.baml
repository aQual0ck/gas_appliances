   M S B A M L   `   `   `  ����I  Egas_appliances, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:gas_appliances.Pagesgas_appliances.Pages  5      '    !gas_appliances.Pages.PageEditUser   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:gas_appliances.Pages     5        N� Title$  PageEditUser��5	      .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $	��430 �6   .+�5   
   o� $��0��6   $��Center=�6   .@�5      m� -   5        $��
Назад��5      6&   m� 5      -   5        $��menuDel��5      $��Удалить��6&   67   5      �� 5   
   $	��350��6   $	��175��6    
����������#666666$��1��5      �� 5   
   $��Center=�6   $��Center=�62   $��1��5      .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��Имя:��6   $	��150��6(   �� 5      -   5        $��txbFullName��5      $	��150��6-   $��
5, 5, 0, 5q�69   ��5      �� *
FullName+5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Логин:��6   $	��150��6*   �� 5       -   5        $��txbUsername��5       $	��150��6-   $��
5, 5, 0, 5q�69   ��5!      �� *
Username+5"      �� 5#      $ 
Horizontal=�6   .+�5$      �� $��Пароль:��6   $	��150��6+   �� 5%      -   5        $��txbPassword��5%      $	��150��6-   $��
5, 5, 0, 5q�69   ��5&      �� *
Password+5'      �� 5(      $ 
Horizontal=�6   .+�5)      �� $��Категория:��6   $	��150��6.   �� 5*      -   5        $��cmbRole��5*      $	��150��6*   $��20��66   $��
5, 5, 0, 5q�6B   5+      �� 5,      -   5        $��btnSave��5,      $��Сохранить��6$    � 	Resources 5-       �� 
TargetType �� Property �� Value��%^   '��       �� 5.      " ��6   .�5/      ��  �@��CornerRadius$ 7��69   50      51      52      53      54      55      