   M S B A M L   `   `   `  ����E  ATemsParser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:TemsParser.BehaviorsTemsParser.Behaviors  5   	   A%clr-namespace:TemsParser.UserControlsTemsParser.UserControls  5   	   =#clr-namespace:TemsParser.ConvertersTemsParser.Converters  5   	       TemsParser.Views.MainView   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   8	behaviors"clr-namespace:TemsParser.Behaviors     5   	   >userControls%clr-namespace:TemsParser.UserControls     5   	   :
converters#clr-namespace:TemsParser.Converters     5   	     � Title$  
TemsParser��5   	    � SizeToContent$ Height=�5	   	   $	��355��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   #��� ��5   	    � 
ResizeMode$ CanMinimize=�5   	   .  (System.Windows.Interactivity.Interaction   	Behaviors 5      1   +TemsParser.Behaviors.WindowGetFocusBehavior  5   
    J� InputBindings 5      �� 5   
    �� Gesture$ CTRL+P��6    �� Command 5      �� *EditConfigCommand+�� 5   
   $ CTRL+O��6    5      �� *OpenCommand+�� 5   
   $ CTRL+R��6    5      �� *ParseCommand+ � 	Resources 5      /   )TemsParser.Converters.JoinStringConverter   joinStringConverter	 �� 
TargetType
 �� Property �� Value �� Path �� Mode �� UpdateSourceTrigger��%�  &	        '��      '�� Y     '�� �       5   
   6)   �� 5   
   "	 ��6   .�5      �� 
 � |� .  6*   5      �� 5   
   "	 ��6   .�5      �� 
 � �� 5       Z� �� 5!      
 � ��# � ��5"      �� 5#      
 � ��$ Disabled=�5$      5%      �� 5&   
   "	 ��6   .�5'      �� 
 � {� 5(      �� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�5)      5*      .��5+      �  � RowDefinitions 5,   
   �� 5-      $
��Auto �6   �� 5.      $
��Auto �6   �� 5/      $
��Auto �6   �� 50      $
��Auto �6   �� 51      $
��Auto �6   .+�53   
   o� $
��Left=�6   $
��auto��54      $��0��55      $	��Top=�56      $
��auto��57      .@�58      m� $��ФАЙЛ��6   .@�59      m� $��Открыть��6    m� InputGestureText$ Ctrl+O��5:       m� Command 5;      �� *OpenCommand+m� 5<      $��Конфигурация��6   $ Ctrl+P��5=       5>      �� *EditConfigCommand+�� 5?      6   m� 5@      $��
Выход��6    5A      �� *ExitCommand+5B      m� 5C      $-��'ВЫЧИСЛИТЬ РАССТОЯНИЕ��6    5D      �� *CalculateDistanceCommand+5E      �� 5F   
   $��
Файлы��6   $��5q�6"   $��1��6.   .��5G      �� 
 � Tag 5H      �� 5I      $ IsOpenedFiles�6   $ OneWay=�5J      $ PropertyChanged=�5K       �� ValidationRules 5L      5  /System.Windows.Controls.DataErrorValidationRule  5M      5O      .+�5Q      �� $��Открыть��6   $��75��5R      $
��Left=�5S       � ToolTip$ CTRL+O��5V      $	��0,5q�5W      ��5T      �� *OpenCommand+{�5U      �� *OpenIsEnabled+$ OneWay=�$ PropertyChanged=�5W      �� 5X      $��25��6   $	��100��5Y      ��5Z      �� $ 	OpenFiles�$ OneWay=�$ PropertyChanged=���5[      �� 5\      .R�5]      �� $
��Left=�6&    �� 
IsReadOnly . $	��Top=�5_   &   $��0q�5`   &   #��� ��5a   &   * �� #IsInactiveSelectionHighlightEnabled .  $��1q�5c   &   .��61    5d   "   3   -TemsParser.Behaviors.TextBoxSelectAllBehavior  5e   &   .��5f   ;   ��5g   "   �� 5h   &   "	 ��6,    �� Triggers 5i   *   P� 5j   .    P� Property �@=�HasError P� Value .  . �5k   6   �� 
 �@�ToolTip 5l   =   �� $
 Path�5m   /   P� 5n   .    �@=�HasError . . �5o   2   �� 
 �@�ToolTip 5p   6   `� 5q   :    `� 	Converter# [  6G    `� ConverterParameter$ . ��6x   .1�5r   >   �� $
 Path�6F   �� 5s   >   $) #(Validation.Errors)[0].ErrorContent�5t   F    �� RelativeSource 5s   F   �� *Self+5t   F   5u   ;   5w   3   5{   /   P� 5|   .    �@J�IsKeyboardFocused .  . �5}   2   �� 
 � �� 5~   6   �� 5   :    �� 
TargetType" ��6J   .R�5�   >   �� #��z��6H   $��CharacterEllipsis=�5�   H   $	��3,1q�5�   H   5�   ;   5�   3   5�   /   5�   '   .��5�   1   ��5�   "   �� 5�   &   $
 Path�6.   $ OneWay=�5�   .   5�   '   .��5�   0    5�   "   �� 5�   &   $ IsExist�6.   $ OneWay=�5�   .   $ PropertyChanged=�5�   .    5�   *     5�   .   5�   '   .��5�   /   5�      5�      5�      5�      5�      �� 5�   
   $��Конфигурация��6   $��5q�5�      $��2��5�      .��5�      �� .+�5�      �� $
��Left=�6   $
��Wrap=�5�      $��Регион��5�      $	��Top=�5�      �� 5�      $
��Left=�6   $	��100��5�       �� 
IsReadOnly . {�5�      �� *	IsEnabled+$ OneWay=�$ PropertyChanged=���5�      �� $ RegionNames�$ OneWay=�$ PropertyChanged=� �� SelectedValue 5�      �� 5�      $ SelectedRegionName�6"   $ TwoWay=�5�   "   $ PropertyChanged=�5�   "    5�        5�   "   5�      5�      � 5�      $	��150��6   $	��0,5q�5�      .+�5�      � $	��Top=�6     � ColumnDefinitions  5�      �� 5�      �� 5�      .+�5�      �� $��0��6%    5�      �� 5�   "   $ IsOperatorSelected�6*   $ OneWay=�5�   &   $ PropertyChanged=�5�   &    5�   &     5�   *   5�   #   .+�5�      �� $
��Left=�6(   $
��Wrap=�5�   (   $��Операторы��5�   (   $	��Top=�5�   (   �� 5�      $	��200��6&   ��5�   "   �� 5�   &   $ 	Operators�6.   $ OneWay=�5�   .   $ PropertyChanged=�5�   .   ��5�   "   �� 5�   &   .R�5�   *   �� $��3q�5�   3   ��5�   3   �� $
 Name�$ OneWay=�{�5�   3   �� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�! d� 	IsChecked! 5�   .   �� 5�   2   $ 	IsChecked�6:   $ TwoWay=�5�   :   $ PropertyChanged=�5�   :   5�   +   5�   '   5�      5�      �� 5�      $��1��6%    5�      �� 5�   "   $ IsTechnologySelected�6*   $ OneWay=�5�   &   $ PropertyChanged=�5�   &    5�   &     5�   *   5�   #   .+�5�      �� $
��Left=�6(   $
��Wrap=�5�       $��Технологии��5�       $	��Top=�5�       �� 5�      $	��200��6&   ��5�   "   �� 5�   &   $ Technologies�6.   $ OneWay=�5�   .   $ PropertyChanged=�5�   .   ��5�   "   �� 5�   &   .R�5�   *   �� $��3q�5�   3   ��5�   3   �� $
 Name�$ OneWay=�{�5�   3   �� *	IsEnabled+! 5�   .   �� 5�   2   $ 	IsChecked�6:   $ TwoWay=�5�   :   $ PropertyChanged=�5�   :   5   +   5  '   5     5     5     5     �� 5     $��Конфигурация��6   $��90��5     $
��Left=�5	     $ CTRL+P��5
     ��5     �� *EditConfigCommand+5     5     �� 5  
   $��Настройки��6   $��5q�5     $��3��5     .��5     �� .+�5     � .+�5     �� $��Биннинг��6   {�5     �� *	IsEnabled+$ OneWay=�$ PropertyChanged=�! 5     �� $ BinningEnabled�$ PropertyChanged=��� 5     " �� TickPlacement$" BottomRight=�5     # �� TickFrequency$# 50Q�5     $��Stretch=�5     $��5,0,0,0q�5     $	��Top=�5     $	��200��5     $ � Minimum$ 5     �� *BinningSizeMinValue+$ OneWay=�% � Maximum% 5     �� *BinningSizeMaxValue+$ OneWay=�{�5     �� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�& � Value& 5     �� $ BinningSizeSlider�$ TwoWay=�$ PropertyChanged=��� 5      $��23��6   $��5,0,0,0q�5!     $��30��5"     {�5#     �� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�.��6u    5$       5%     .��5&  3   ��5'     �� 5(     $ BinningSizeTextBox�6&   $ PropertyChanged=�5)  &   ' �� NotifyOnValidationError' .  5+  "     5,  &   5.     .��5/  (   50     �� 51     $
��Left=�6    $
��Wrap=�52      ( �� Padding$	( 0,3q�53      $��м��54      $	��Top=�55      56     �� 57     $-��'Сравнение операторов��6   $��23��58     ! 59     �� 5:     $ CompareOperatorsEnabled�6"   $ TwoWay=�5;  "   $ PropertyChanged=�5<  "   {�5>     �� 5?     $( "CompareOperatorsCheckBoxVisibility�6"   $ OneWay=�5@  "   $ PropertyChanged=�5A  "   5C     �� 5D     $)��#Определять частоту��6   $��23��5E     ! 5F     �� 5G     $ DefineFreqEnabled�6"   $ TwoWay=�5H  "   $ PropertyChanged=�5I  "   5K     5L     5M     �� 5N  
   $��Выполнение��6   $��5q�5O     $��4��5P     .��5Q     � .+�5R     �� $��Выполнить��6   $
��Left=�5S     $	��Top=�5T     $	��100��5U     $ CTRL+R��5V     ��5W     �� *ParseCommand+5X     5Y     5Z     5[     