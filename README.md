# SignalRChat
-    Cách thức hoạt động của signalR:<br><br>
 1.SignalR quản lý kết nối 1 cách tự động, truyền thông điệp tới các client đã  kết nối tới<br>
 2.SignalR dùng hub cho phép client gọi tới các phương thức được định nghĩa ở server<br>
 3.Bên server sẽ tạo các public class kế thừa từ 1 lớp trừu tượng Hub, các hub class được map với 1 path<br>
 4.Bên client để gọi đến phương thức của hub class bên server sẽ phải tạo 1 connection với url bao gồm path tương ứng, kết nối này là liên tục<br>
 5.Client để connection này nhận thông điệp từ hubclass bên server mỗi khi client gọi đến phương thức của hub class<br>
-    Quá trình kết nối của SignalR:<br> <br>
-Server<br>
1.ConfigureServices trong startup.cs (add SignalR,AddCors)<br>
2.Tạo các method trong hub class và config endpoint Maphub<br><br>
-Client(tùy vào javascript client hay .NET client hay java client mà cách gọi khác nhau) <br>
 1.Tạo connection từ Client đến hub class bên server, với url xác định <br>
 2.dùng phương thức .on() của HubConnection để nhận thông điệp từ hub<br>
 3.hubConnection.start() để bắt đầu tạo kết nối.<br>
 4.hubConnection.invoke() để gọi tới hàm trong hub class<br>
 
 
