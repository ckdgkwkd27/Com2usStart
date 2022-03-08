## 피드백 반영을 통한 코드 개선(22.03.08)

### 추가된 내용(22.03.08)
- DB 관련 클래스들 생성자에서 바로 연결 수립
- 불필요한 매니저 클래스 삭제
- Attend, AttendGift Controller 결합
- 선물지급 코드 함수화해서 전체 흐름을 간략화
- Controller마다 Request, Response 생성자에서 바로 디폴트 에러코드 지정
- 쿼리함수 이름 알맞게 수정
- Player ID를 VARCHAR에서 Auto-Increment INT로 변경
- 클라이언트에게 불필요한 정보 제공 삭제
- Dispose 패턴에서 소멸자 코드 삭제
<br/>

### 사용법
```
http://localhost:7038/Join: (회원가입) ID, Password 값을 전달하고 DB에 저장합니다.

http://localhost:7038/Login: (로그인)  ID, Password 값을 전달하고 
DB에서 계정이 유효한지 확인합니다. 

http://localhost:7038/PlayerCreate: (플레이어 생성)  ID, AuthToken 값을 전달하고 
일치하면 플레이어를 생성합니다.

http://localhost:7038/DataLoad: (데이터 로드)  ID, PlayerID, AuthToken 값을 전달하고 
일치하면 플레이어 데이터를 로드합니다.

http://localhost:7038/Attend: (출석) ID, PlayerID, AuthToken 값을 전달하고 일치하면 
DB에 출석기록을 추가/업데이트 합니다.

http://localhost:7038/AttendGift (출석보상) ID, PlayerID, AuthToken 값을 전달, 일치여부를 확인합니다.
플레이어가 보상을 받은지 일정기간이 지났다면 보상을 지급하고 편지를 발송합니다.

http://localhost:7038/Mail (우편함) ID, PlayerID, AuthToken 값을 전달하고 일치여부를 확인합니다. 
수신된 모든 메일을 보여줍니다.

http://localhost:7038/ReceiveMail (우편수령) ID, PlayerID, AuthToken 값을 전달하고 일치여부를 확인합니다. 
수신된 모든 메일의 보상을 인벤토리로 수령하고 메일을 삭제합니다.

http://localhost:7038/Inventory (인벤토리) ID, PlayerID, AuthToken 값을 전달하고 일치여부를 확인합니다. 
인벤토리 정보를 출력합니다.
```
<br/>

### 로봇몬고 ER-Diagram
![ERD](https://user-images.githubusercontent.com/30414979/155935871-7de10736-eedf-455f-8bb9-874fe9ecdb01.png)



<br/><br/>
### 테이블 설명
- Account: 사용자의 계정 정보가 저장됩니다.
- GamePlayer: 사용자의 인게임 정보가 저장됩니다.
- Robotmon: 로봇몬들의 정보가 저장됩니다.
- Robotmon_Upgrade: 로못본들의 진화, 강화 정보가 저장됩니다.
- PlayerRobotmon: 플레이어가 잡은 로봇몬의 정보들이 저장됩니다.
- Item: 아이템 관련 정보들이 저장됩니다.
- Mail: 발송된 메일들에 대한 정보가 저장됩니다.
- Inventory: 사용자의 인벤토리 정보가 출력됩니다.
<br/>

😀 감사합니다 😀      
