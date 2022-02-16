## 로그인 회원가입 이후 플레이어 데이터 생성 및 로딩(22.02.16)

### 사용법
```
http://localhost:7038/Join: (회원가입) ID, Password 값을 전달하고 DB에 저장합니다.
http://localhost:7038/Login: (로그인)  ID, Password 값을 전달하고 DB에서 계정이 유효한지 확인합니다. 
http://localhost:7038/DataLoad: (게임데이터 로드)  ID, AuthToken 값을 전달하고 일치하면 게임 데이터를 로드해줍니다.
```
<br/>

### 로봇몬고 ER-Diagram
![ERD](https://user-images.githubusercontent.com/30414979/154002511-7fa5514e-5fb1-4a5c-a280-5381e714f9f9.png)

<br/><br/>
### 테이블 설명
- Account: 사용자의 계정 정보가 저장됩니다.
- GamePlayer: 사용자의 인게임 정보가 저장됩니다.
- Robotmon: 로봇몬들의 정보가 저장됩니다.
- PlayerRobotmon: 플레이어가 잡은 로봇몬의 정보들이 저장됩니다.
<br/>
 
### 추가된 내용(22.02.16)
- LoginController에서 게임데이터 로딩 부분 분리
- 게임 데이터를 로딩하는 DataLoadController 생성. ID와 AuthToken을 가지고 검증후 데이터 로드
- 암호화 하는 부분 MysqlManager에서 분리. CryptoManager를 생성
- 하나의 함수가 하나의 일만 하도록 분리
- 코딩 룰을 하나로 통일화
 
<br/>
😀 감사합니다 😀      
