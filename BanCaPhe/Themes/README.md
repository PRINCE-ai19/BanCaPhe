# WPF Design System - Caffeine POS

## 📋 Cấu trúc Design System

Design System được tổ chức thành các ResourceDictionary riêng biệt, dễ bảo trì và mở rộng.

### **Tệp tin chính:**

```
Themes/
├── DesignSystem.xaml          ← Merge tất cả (dùng trong App.xaml)
├── Colors.xaml                ← Định nghĩa tất cả màu (Color resources)
├── Brushes.xaml               ← SolidColorBrush từ Colors
├── Typography.xaml            ← Font styles (Outfit, Plus Jakarta Sans)
├── Animations.xaml            ← Storyboards cho interactions
├── Buttons.xaml               ← Button styles (Primary, Secondary, Outline, Success, Error)
├── Inputs.xaml                ← TextBox, ComboBox styles
├── Cards.xaml                 ← Card styles (Product, Stat, Topping, Bento)
├── Modals.xaml                ← Modal/Dialog styles
└── README.md                  ← File này
```

---

## 🎨 Design Tokens

### **Màu sắc (Semantic Naming)**

| Token | Hex | Mục đích |
|-------|-----|---------|
| **Primary** | #0058BE | Hành động chính, nút bấm |
| **Secondary** | #5C5F60 | Trung tính, phụ |
| **Tertiary** | #006947 | Thành công, xác nhận |
| **Error** | #BA1A1A | Lỗi, cảnh báo |
| **Surface** | #F9F9F9 | Nền chính |
| **Outline** | #727785 | Viền, phân chia |

### **Typography**

- **Headline**: Outfit (Bold, 24-57px)
- **Body**: Plus Jakarta Sans (Regular, 12-16px)
- **Label**: Plus Jakarta Sans (Medium, 11-14px)

### **Animations**

- **Snap Interaction**: 150ms cubic-bezier(0.4, 0, 0.2, 1)
- **Hover**: Scale 1.05 + color change
- **Pressed**: Scale 0.95
- **Disabled**: Opacity 0.6

---

## 🔧 Cách sử dụng

### **1. Trong App.xaml (đã cấu hình)**

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Themes/DesignSystem.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### **2. Sử dụng Brushes trong XAML**

```xml
<!-- Màu nền -->
<Border Background="{StaticResource SurfaceContainerLowestBrush}"/>

<!-- Màu chữ -->
<TextBlock Foreground="{StaticResource OnSurfaceBrush}"/>

<!-- Màu viền -->
<Border BorderBrush="{StaticResource OutlineBrush}"/>
```

### **3. Sử dụng Button Styles**

```xml
<!-- Primary Button -->
<Button Content="Thanh toán" Style="{StaticResource PrimaryButtonStyle}"/>

<!-- Secondary Button -->
<Button Content="Hủy" Style="{StaticResource SecondaryButtonStyle}"/>

<!-- Outline Button -->
<Button Content="Xóa" Style="{StaticResource OutlineButtonStyle}"/>

<!-- Success Button -->
<Button Content="Xác nhận" Style="{StaticResource SuccessButtonStyle}"/>

<!-- Error Button -->
<Button Content="Xóa vĩnh viễn" Style="{StaticResource ErrorButtonStyle}"/>
```

### **4. Sử dụng TextBox Styles**

```xml
<!-- Default TextBox -->
<TextBox Style="{StaticResource DefaultTextBoxStyle}" Placeholder="Nhập tên..."/>

<!-- Error TextBox -->
<TextBox Style="{StaticResource ErrorTextBoxStyle}" Text="Invalid"/>

<!-- Success TextBox -->
<TextBox Style="{StaticResource SuccessTextBoxStyle}" Text="Valid"/>
```

### **5. Sử dụng Card Styles**

```xml
<!-- Product Card -->
<Border Style="{StaticResource ProductCardStyle}">
    <!-- Content -->
</Border>

<!-- Stat Card (Dashboard) -->
<Border Style="{StaticResource StatCardStyle}">
    <!-- Content -->
</Border>

<!-- Bento Card (Action) -->
<Border Style="{StaticResource BentoCardStyle}">
    <!-- Content -->
</Border>
```

### **6. Sử dụng Typography**

```xml
<!-- Headline Large -->
<TextBlock Text="Tiêu đề" Style="{StaticResource HeadlineLargeTextBlock}"/>

<!-- Body Medium -->
<TextBlock Text="Nội dung" Style="{StaticResource BodyMediumTextBlock}"/>

<!-- Label Small -->
<TextBlock Text="Nhãn" Style="{StaticResource LabelSmallTextBlock}"/>
```

---

## 📦 Component Library

### **Buttons**
- ✅ Primary (solid, hover, pressed, disabled)
- ✅ Secondary (muted)
- ✅ Outline (border)
- ✅ Success (green)
- ✅ Error (red)

### **Inputs**
- ✅ TextBox Default
- ✅ TextBox Focused (border highlight)
- ✅ TextBox Error (red border)
- ✅ TextBox Success (green border)
- ✅ ComboBox Default

### **Cards**
- ✅ Product Card (POS)
- ✅ Stat Card (Dashboard)
- ✅ Topping Card
- ✅ Bento Card (Action)
- ✅ Elevated Card

### **Modals**
- ✅ Modal Window Style
- ✅ Modal Header Border (accent bar)
- ✅ Modal Content Border
- ✅ Modal Overlay (40% black)
- ✅ Modal Title TextBlock
- ✅ Modal Description TextBlock

---

## 🎯 Best Practices

### **1. Luôn dùng StaticResource**
```xml
<!-- ✅ Đúng -->
<Border Background="{StaticResource PrimaryBrush}"/>

<!-- ❌ Sai -->
<Border Background="#0058BE"/>
```

### **2. Tách Colors và Brushes**
- Colors: Định nghĩa giá trị hex
- Brushes: Tạo SolidColorBrush từ Colors
- Lợi ích: Dễ thay đổi toàn bộ theme

### **3. Sử dụng BasedOn cho Styles**
```xml
<!-- ✅ Đúng - Tái sử dụng -->
<Style x:Key="SuccessButtonStyle" TargetType="Button" 
       BasedOn="{StaticResource PrimaryButtonStyle}">
    <Setter Property="Background" Value="{StaticResource TertiaryBrush}"/>
</Style>

<!-- ❌ Sai - Lặp code -->
<Style x:Key="SuccessButtonStyle" TargetType="Button">
    <!-- Tất cả properties lặp lại -->
</Style>
```

### **4. Animations - 150ms Snap Feel**
```xml
<!-- Tất cả animations dùng 150ms -->
<DoubleAnimation Duration="0:0:0.150"/>
```

### **5. CornerRadius - 6-10px**
```xml
<!-- Consistent rounding -->
<Border CornerRadius="8"/>
<Border CornerRadius="10"/>
<Border CornerRadius="12"/>
```

---

## 🔄 Cách mở rộng Design System

### **Thêm màu mới:**

1. Thêm vào `Colors.xaml`:
```xml
<Color x:Key="NewColorName">#XXXXXX</Color>
```

2. Thêm vào `Brushes.xaml`:
```xml
<SolidColorBrush x:Key="NewColorBrush" Color="{StaticResource NewColorName}"/>
```

3. Sử dụng:
```xml
<Border Background="{StaticResource NewColorBrush}"/>
```

### **Thêm Button Style mới:**

1. Thêm vào `Buttons.xaml`:
```xml
<Style x:Key="NewButtonStyle" TargetType="Button" BasedOn="{StaticResource PrimaryButtonStyle}">
    <Setter Property="Background" Value="{StaticResource NewColorBrush}"/>
    <!-- Triggers, animations... -->
</Style>
```

2. Sử dụng:
```xml
<Button Style="{StaticResource NewButtonStyle}"/>
```

---

## 📊 Áp dụng vào UI hiện tại

### **MainWindow.xaml**
- ✅ Background: `BackgroundBrush`
- ✅ Buttons: `PrimaryButtonStyle`, `SecondaryButtonStyle`, `SuccessButtonStyle`
- ✅ Cards: `ProductCardStyle`, `ToppingCardStyle`
- ✅ TextBlocks: Semantic colors

### **AdminDashboard.xaml**
- ✅ Background: `BackgroundBrush`
- ✅ Stat Cards: `StatCardStyle`
- ✅ Logout Card: `ErrorContainerBrush`

---

## 🚀 Tiếp theo

1. **Áp dụng vào các UserControl khác** (UC_SanPham, UC_NhanVien, v.v.)
2. **Tạo Dark Mode** (thêm Colors.Dark.xaml)
3. **Thêm Animation Triggers** cho hover/pressed states
4. **Tối ưu Performance** (merge dictionaries thông minh)

---

## 📝 Ghi chú

- Tất cả animations dùng **150ms cubic-bezier(0.4, 0, 0.2, 1)** - "snap feel"
- CornerRadius: **8px** (buttons), **10px** (cards), **12px** (containers)
- Opacity overlay: **40%** (modals)
- Font: **Outfit** (headlines), **Plus Jakarta Sans** (body/labels)

---

**Design System v1.0 - Caffeine POS**
