   M S B A M L   `   `   `  ���� E  ATemsParser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:TemsParser.BehaviorsTemsParser.Behaviors  A%clr-namespace:TemsParser.UserControlsTemsParser.UserControls  =#clr-namespace:TemsParser.ConvertersTemsParser.Converters      TemsParser.Views.MainView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     8	behaviors"clr-namespace:TemsParser.Behaviors     >userControls%clr-namespace:TemsParser.UserControls     :
converters#clr-namespace:TemsParser.Converters       � Title$  
TemsParser�� � SizeToContent$ Height=�$	��355�� � WindowStartupLocation$ CenterScreen=�#��� �� � 
ResizeMode$ CanMinimize=�.  (System.Windows.Interactivity.Interaction   	Behaviors 1   +TemsParser.Behaviors.WindowGetFocusBehavior   J� InputBindings ��  �� Gesture$ CTRL+P�� �� Command �� *EditConfigCommand+�� $ CTRL+O�� �� *OpenCommand+�� $ CTRL+R�� �� *ParseCommand+ � 	Resources /   )TemsParser.Converters.JoinStringConverter   joinStringConverter	 �� 
TargetType
 �� Property �� Value �� Path �� Mode �� UpdateSourceTrigger��%�   &	        '��      '�� &     '�� y       �� "	 ��.��� 
 � |� .  �� "	 ��.��� 
 � �� Z� �� 
 � ��# � ���� 
 � ��$ Disabled=��� "	 ��.��� 
 � {� �� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�o� $
��Left=�$
��auto��$��0��$	��Top=�$
��auto��.@�m� $��ФАЙЛ��.@�m� $��Открыть�� m� InputGestureText$ Ctrl+O�� m� Command �� *OpenCommand+m� $��Конфигурация��$ Ctrl+P�� �� *EditConfigCommand+�� m� $��
Выход�� �� *ExitCommand+m� $-��'ВЫЧИСЛИТЬ РАССТОЯНИЕ�� �� *CalculateDistanceCommand+�� $��
Файлы��$��5q�$��1��.���� 
 � Tag �� $ IsOpenedFiles�$ OneWay=�$ PropertyChanged=� �� ValidationRules 5  /System.Windows.Controls.DataErrorValidationRule  .+��� $��Открыть��$��75��$
��Left=� � ToolTip$ CTRL+O��$	��0,5q����� *OpenCommand+{��� *OpenIsEnabled+$ OneWay=�$ PropertyChanged=��� $��25��$	��100������ $ 	OpenFiles�$ OneWay=�$ PropertyChanged=����� .R��� $
��Left=� �� 
IsReadOnly . $	��Top=�$��0q�#��� ��* �� #IsInactiveSelectionHighlightEnabled .  $��1q�.�� 3   -TemsParser.Behaviors.TextBoxSelectAllBehavior  .������ "	 �� �� Triggers P�  P� Property �@=�HasError P� Value .  . ��� 
 �@�ToolTip �� $
 Path�P�  �@=�HasError . . ��� 
 �@�ToolTip `�  `� 	Converter# [   `� ConverterParameter$ . ��.1��� $
 Path��� $) #(Validation.Errors)[0].ErrorContent� �� RelativeSource �� *Self+P�  �@J�IsKeyboardFocused .  . ��� 
 � �� ��  �� 
TargetType" ��.R��� #��z��$��CharacterEllipsis=�$	��3,1q�.������ $
 Path�$ OneWay=�.�� �� $ IsExist�$ OneWay=�$ PropertyChanged=�   .���� $��Конфигурация��$��5q�$��2��.���� .+��� $
��Left=�$
��Wrap=�$��Регион��$	��Top=��� $
��Left=�$	��100�� �� 
IsReadOnly . {��� *	IsEnabled+$ OneWay=�$ PropertyChanged=����� $ RegionNames�$ OneWay=�$ PropertyChanged=� �� SelectedValue �� $ SelectedRegionName�$ TwoWay=�$ PropertyChanged=�   � $	��150��$	��0,5q�.+�� $	��Top=�  � ColumnDefinitions  �� �� .+��� $��0�� �� $ IsOperatorSelected�$ OneWay=�$ PropertyChanged=�   .+��� $
��Left=�$
��Wrap=�$��Операторы��$	��Top=��� $	��200������ $ 	Operators�$ OneWay=�$ PropertyChanged=����� .R��� $��3q����� $
 Name�$ OneWay=�{��� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�! d� 	IsChecked! �� $ 	IsChecked�$ TwoWay=�$ PropertyChanged=��� $��1�� �� $ IsTechnologySelected�$ OneWay=�$ PropertyChanged=�   .+��� $
��Left=�$
��Wrap=�$��Технологии��$	��Top=��� $	��200������ $ Technologies�$ OneWay=�$ PropertyChanged=����� .R��� $��3q����� $
 Name�$ OneWay=�{��� *	IsEnabled+! �� $ 	IsChecked�$ TwoWay=�$ PropertyChanged=��� $��Конфигурация��$��90��$
��Left=�$ CTRL+P������ *EditConfigCommand+�� $��Настройки��$��5q�$��3��.���� .+�� .+��� $��Биннинг��{��� *	IsEnabled+$ OneWay=�$ PropertyChanged=�! �� $ BinningEnabled�$ PropertyChanged=��� " �� TickPlacement$" BottomRight=�# �� TickFrequency$# 50Q�$��Stretch=�$��5,0,0,0q�$	��Top=�$	��200��$ � Minimum$ �� *BinningSizeMinValue+$ OneWay=�% � Maximum% �� *BinningSizeMaxValue+$ OneWay=�{��� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�& � Value& �� $ BinningSizeSlider�$ TwoWay=�$ PropertyChanged=��� $��23��$��5,0,0,0q�$��30��{��� $ 	IsEnabled�$ OneWay=�$ PropertyChanged=�.��   .������ $ BinningSizeTextBox�$ PropertyChanged=�' �� NotifyOnValidationError' .    .���� $
��Left=�$
��Wrap=�( �� Padding$	( 0,3q�$��м��$	��Top=��� $-��'Сравнение операторов��$��23��! �� $ CompareOperatorsEnabled�$ TwoWay=�$ PropertyChanged=�{��� $( "CompareOperatorsCheckBoxVisibility�$ OneWay=�$ PropertyChanged=��� $)��#Определять частоту��$��23��! �� $ DefineFreqEnabled�$ TwoWay=�$ PropertyChanged=��� $��Выполнение��$��5q�$��4��.��� .+��� $��Выполнить��$
��Left=�$	��Top=�$	��100��$ CTRL+R������ *ParseCommand+