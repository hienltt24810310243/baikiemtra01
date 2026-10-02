#### Thông tin sinh viên:
    - Họ và tên: Lê Thị Thúy Hiền
    - Mã sinh viên: 24810310243
    - Lớp: D19CNPM3

#### Câu 1: Trình bày sự khác nhau giữa Value Types (Kiểu giá trị) và Reference Types(Kiểu tham chiếu) trong C# về cơ chế lưu trữ vùng nhớ (Stack vs Heap).

-Value Types (Kiểu giá trị)
+ Vị trí lưu trữ: Cấp phát trên Stack (nếu là biến cục bộ). Lưu ý: Nếu nằm trong một Reference Type, nó sẽ được lưu trên Heap cùng đối tượng đó.
+ Nội dung lưu trữ: Chứa trực tiếp giá trị thực (ví dụ: số 10, ký tự 'A').
+ Khi gán biến: Tạo ra một bản sao hoàn toàn độc lập. Thay đổi biến mới không ảnh hưởng đến biến cũ.
+ Giải phóng bộ nhớ: Tự động giải phóng ngay khi thoát khỏi phạm vi (scope) của hàm/khối lệnh. Tốc độ truy xuất và dọn dẹp rất nhanh.
+ Bao gồm: Các kiểu dữ liệu cơ bản (int, float, double, bool, char), struct, enum.

- Reference Types (Kiểu tham chiếu)
+ Vị trí lưu trữ: Đối tượng thực tế được cấp phát trên Heap, còn biến chứa địa chỉ tham chiếu tới đối tượng đó được lưu trên Stack.
+ Nội dung lưu trữ: Chứa địa chỉ bộ nhớ (con trỏ) trỏ tới vị trí của dữ liệu trên Heap.
+ Khi gán biến: Chỉ sao chép địa chỉ tham chiếu. Cả hai biến sẽ cùng trỏ vào một đối tượng chung trên Heap, thay đổi một biến sẽ làm thay đổi biến kia.
+Giải phóng bộ nhớ: Do bộ thu gom rác (Garbage Collector - GC) quản lý và tự động dọn dẹp khi đối tượng không còn được ai tham chiếu tới.
+ Bao gồm: class, interface, delegate, object, string, các loại mảng (Array).

#### Câu 2: Tính năng Init-only Properties (init) trong C# 9/10 khác gì so với thuộc tínhcó set thông thường? Nêu trường hợp sử dụng thực tế.
-Sự khác biệt cốt lõi
+ Thuộc tính dùng set thông thường: Cho phép gán và thay đổi giá trị của thuộc tính ở bất kỳ thời điểm nào trong suốt vòng đời của đối tượng, kể cả sau khi đối tượng đã được tạo xong (tính khả biến - mutable).
+ Thuộc tính dùng init (Init-only): Chỉ cho phép gán giá trị duy nhất một lần trong giai đoạn khởi tạo đối tượng (thông qua hàm tạo Constructor hoặc cú pháp Object Initializer {}). Ngay sau khi lệnh khởi tạo hoàn tất, thuộc tính đó lập tức trở thành chỉ đọc (read-only), mọi hành động cố tình gán lại giá trị sẽ bị trình biên dịch báo lỗi (tính bất biến - immutable).

-Trường hợp sử dụng thực tế
+ Data Transfer Objects (DTOs) và ViewModels: Khi vận chuyển dữ liệu giữa các tầng (layers) của ứng dụng (ví dụ: từ Database lên Controller, hoặc qua REST API), đối tượng chỉ mang ý nghĩa "chứa dữ liệu". Dùng init đảm bảo dữ liệu không bị ai đó vô tình sửa đổi dọc đường.
+ Bảo vệ các trường dữ liệu hệ thống (System Fields): Các thuộc tính định danh như Id, CreatedDate, CreatedBy của một bản ghi cơ sở dữ liệu chỉ nên được gán một lần lúc ánh xạ (mapping) dữ liệu và tuyệt đối không được phép chỉnh sửa trong các logic nghiệp vụ (business logic) phía sau.
+ Lập trình đa luồng (Multi-threading): Tạo ra các đối tượng bất biến (Immutable Objects). Khi trạng thái của đối tượng không thể thay đổi sau khi tạo, nhiều luồng (threads) có thể cùng đọc dữ liệu từ đối tượng đó mà không sợ xảy ra xung đột (race condition), giúp bỏ qua các cơ chế khóa (lock) phức tạp.

#### Câu 3: Phân biệt sự khác nhau giữa phương thức virtual ở lớp cha và phươg thức override ở lớp con khi triển khai tính Đa hình (Polymorphism).
- Phương thức virtual (Lớp cha - Base Class)
+ Mục đích: Cấp quyền cho các lớp kế thừa được phép thay đổi hoặc viết lại logic của phương thức.
+ Đặc tính: Luôn đi kèm với một phần thân (body) chứa đoạn code xử lý mặc định.
+ Sự ràng buộc: Không bắt buộc lớp con phải thay đổi. Nếu lớp con không làm gì, đối tượng của lớp con sẽ tự động sử dụng logic mặc định đã định nghĩa ở phương thức virtual.

- Phương thức override (Lớp con - Derived Class)
+ Mục đích: Cung cấp một định nghĩa mới (ghi đè) để thay thế hoàn toàn logic của phương thức ở lớp cha.
+ Đặc tính: Phải có chung chữ ký (signature: tên, tham số, kiểu trả về) với phương thức ở lớp cha.
+Sự ràng buộc: Chỉ có thể dùng từ khóa override khi phương thức ở lớp cha đã được đánh dấu là virtual, abstract, hoặc bản thân nó đang override từ một lớp cao hơn.

- Cơ chế hoạt động khi triển khai Đa hình (Runtime Polymorphism)
+ Vấn đề: Khi khai báo biến tham chiếu kiểu Lớp Cha nhưng trỏ tới đối tượng thực tế là Lớp Con (ví dụ: BaseClass obj = new DerivedClass();).
+ Cách giải quyết: Khi gọi obj.Method(), Common Language Runtime (CLR) sẽ áp dụng kỹ thuật Late Binding (Liên kết trễ). Nó không nhìn vào kiểu của biến (BaseClass), mà nhìn vào kiểu thực sự của đối tượng trong bộ nhớ Heap lúc chương trình đang chạy (DerivedClass).
+ Kết quả: Nhờ cặp virtual/override, CLR tự động bỏ qua phương thức gốc và gọi chính xác phiên bản phương thức đã được override ở lớp con.

#### Câu 4: Tại sao một thành phần được khai báo là static trong Lớp (Class) lại không thể truy xuất thông qua một thể hiện (Object Instance) được tạo bằng toán tử new?
- Lý do một thành phần static không thể truy cập thông qua một thể hiện (instance) xuất phát từ cơ chế quản lý bộ nhớ và triết lý thiết kế minh bạch của ngôn ngữ C#:
+ Bản chất sở hữu (Type vs. Instance): Thành phần static thuộc về chính định nghĩa của Lớp (Class / Type), chứ không thuộc về bất kỳ đối tượng cụ thể nào. Khi bạn dùng toán tử new, bạn tạo ra một bản sao dữ liệu cá nhân cho đối tượng đó, nhưng thành phần static là tài nguyên dùng chung của toàn bộ hệ thống, không nằm trong "bản sao" này.

+ Vùng nhớ lưu trữ độc lập:
    a.Các đối tượng tạo bằng new được cấp phát trên bộ nhớ GC Heap và có vòng đời phụ thuộc vào tham chiếu.
    b.Các thành phần static được nạp một lần duy nhất vào một vùng nhớ đặc biệt dành riêng cho quản lý cấu trúc kiểu dữ liệu (Type Object / High-Frequency Heap) khi Lớp (Class) lần đầu được load vào Application Domain. Nó tồn tại xuyên suốt chương trình.

+ Ngăn chặn nhầm lẫn logic (Triết lý C#): Nếu C# cho phép viết myObject.StaticProperty = 10;, người đọc code sẽ rất dễ nhầm tưởng rằng chỉ trạng thái của riêng myObject bị thay đổi. Tuy nhiên, hành động đó thực chất đang sửa đổi trạng thái toàn cục của toàn bộ ứng dụng. Sự mập mờ này rất dễ sinh ra bug.

+ Khóa cứng từ Trình biên dịch (Compiler): Rút kinh nghiệm từ các ngôn ngữ cũ hơn (như Java cho phép gọi static qua instance dù cảnh báo), C# thiết kế trình biên dịch khắt khe hơn. Nó sẽ chủ động chặn hành vi này và văng lỗi biên dịch CS0176. Bạn bắt buộc phải gọi thông qua tên Lớp (ví dụ: ClassName.StaticMember) để code luôn thể hiện rõ ràng ý định can thiệp vào tài nguyên dùng chung.