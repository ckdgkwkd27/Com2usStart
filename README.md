## 출석, 출석보상, 우편함 구현(22.02.18)

### 사용법
```
http://localhost:7038/Join: (회원가입) ID, Password 값을 전달하고 DB에 저장합니다.

http://localhost:7038/Login: (로그인)  ID, Password 값을 전달하고 DB에서 계정이 유효한지 확인합니다. 

http://localhost:7038/DataLoad: (데이터 로드)  ID, AuthToken 값을 전달하고 일치하면 게임 데이터를 로드합니다.

http://localhost:7038/Attend: (출석) ID, AuthToken 값을 전달하고 일치하면 DB에 출석기록을 추가/업데이트 합니다.

http://localhost:7038/AttendGift (출석보상) ID, AuthToken 값을 전달하고 일치여부를 확인합니다. 플레이어가 보상을 받은지 
일정기간이 지났다면 보상내역을 업데이트하고 편지를 발송합니다.

http://localhost:7038/Mail (우편함) ID, AuthToken 값을 전달하고 일치여부를 확인합니다. 플레이어에게 수신된 모든 메일을 보여줍니다.
```
<br/>

### 로봇몬고 ER-Diagram
![ERD](https://user-images.githubusercontent.com/30414979/154617673-7fab160f-55a1-4c45-8589-aab398b4916e.png)


<br/><br/>
### 테이블 설명
- Account: 사용자의 계정 정보가 저장됩니다.
- GamePlayer: 사용자의 인게임 정보가 저장됩니다.
- Robotmon: 로봇몬들의 정보가 저장됩니다.
- PlayerRobotmon: 플레이어가 잡은 로봇몬의 정보들이 저장됩니다.
- item: 아이템 관련 정보들이 저장됩니다.
- Attendance: 사용자의 출석 정보가 저장됩니다.
- Mail: 발송된 메일들에 대한 정보가 저장됩니다.
<br/>
 
### 추가된 내용(22.02.18)
- 비동기 호출 부분(async, await) 전체적으로 개선
- AuthToken 인증부분 RedisManager에서 담당.
- 출석부(AttendController) 기능 추가
- 출석보상(AttendGiftController) 기능 추가. 일정기간(현재는 1분)이 지나면 출석보상 메일 발송
- 우편함(MailController) 기능 추가. 사용자에게 수신된 모든 메일에 대한 결과 리턴
 
<br/>
😀 감사합니다 😀      
