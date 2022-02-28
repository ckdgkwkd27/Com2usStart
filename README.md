## 메일수령, 인벤토리 구현 및 기획테이블 파싱,db저장 기능(22.02.28)

### 사용법
```
http://localhost:7038/Join: (회원가입) ID, Password 값을 전달하고 DB에 저장합니다.

http://localhost:7038/Login: (로그인)  ID, Password 값을 전달하고 
DB에서 계정이 유효한지 확인합니다. 

http://localhost:7038/PlayerCreate: (플레이어 생성)  ID, AuthToken 값을 전달하고 
일치하면 플레이어를 생성합니다.

http://localhost:7038/DataLoad: (데이터 로드)  ID, UUID, AuthToken 값을 전달하고 
일치하면 플레이어 데이터를 로드합니다.

http://localhost:7038/Attend: (출석) ID, UUID, AuthToken 값을 전달하고 일치하면 
DB에 출석기록을 추가/업데이트 합니다.

http://localhost:7038/AttendGift (출석보상) ID, UUID, AuthToken 값을 전달, 일치여부를 확인합니다.
플레이어가 보상을 받은지 일정기간이 지났다면 보상을 지급하고 편지를 발송합니다.

http://localhost:7038/Mail (우편함) ID, UUID, AuthToken 값을 전달하고 일치여부를 확인합니다. 
수신된 모든 메일을 보여줍니다.

http://localhost:7038/ReceiveMail (우편수령) ID, UUID, AuthToken 값을 전달하고 일치여부를 확인합니다. 
수신된 모든 메일의 보상을 인벤토리로 수령하고 메일을 삭제합니다.

http://localhost:7038/Inventory (인벤토리) ID, UUID, AuthToken 값을 전달하고 일치여부를 확인합니다. 
인벤토리 정보를 출력합니다.
```
<br/>

### 로봇몬고 ER-Diagram
![ERD](https://user-images.githubusercontent.com/30414979/155935642-cb5c772f-a2c3-4d3e-b913-cebdc51fb496.png)



<br/><br/>
### 테이블 설명
- Account: 사용자의 계정 정보가 저장됩니다.
- GamePlayer: 사용자의 인게임 정보가 저장됩니다.
- Robotmon: 로봇몬들의 정보가 저장됩니다.
- PlayerRobotmon: 플레이어가 잡은 로봇몬의 정보들이 저장됩니다.
- Item: 아이템 관련 정보들이 저장됩니다.
- Mail: 발송된 메일들에 대한 정보가 저장됩니다.
- Inventory: 사용자의 인벤토리 정보가 출력됩니다.
<br/>

### 추가된 내용(22.02.24)
- 메일을 수령하는 ReceiveMail 컨트롤러 추가
- 사용자의 아이템 정보를 보여주는 Inventory 컨트롤러 추가
- Redis 토큰 인증을 담당하는 TokenCheck 미들웨어 추가
- DB테이블 전체적으로 다 개선 
- 장황한 함수 호출부분 최대한 간략화(세부,대략등 원하는 관점에 따라 볼수있게)
- 컨트롤러 부분 의존성 주입(DI) 추가
- MysqlManager의 Connection부분을 따로빼서(RealDbConnector) DI로 구현. MysqlManager는 쿼리처리만 담당
- MysqlManager에서 Dispose 패턴 구현. Dispose시 Connection Close처리 

### 추가된 내용(22.02.28)
- 로봇몬 정보, 진화 정보, 강화 정보를 저장하는 테이블(csv) 추가
- 해당 테이블들을 파싱하고 db에 저장하는 클래스(--Impl.cs)들 추가
<br/>
😀 감사합니다 😀      
