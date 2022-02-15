## 로그인 회원가입 이후 플레이어 데이터 생성 및 로딩(22.02.15)

### 사용법
`http://localhost:7038/Join: (회원가입) _ID, _Password에 계정정보를 전달하고 DB에 저장합니다.`   
`http://localhost:7038/Login: (로그인)  _ID, _Password에 계정정보를 전달하고 DB에서 계정이 유효한지 확인합니다.`                                                                       
<br/>

### 로봇몬고 ER-Diagram
![ERD](https://user-images.githubusercontent.com/30414979/154002511-7fa5514e-5fb1-4a5c-a280-5381e714f9f9.png)

<br/><br/>
### 추가된 내용(22.02.15)
- Join시 Game Data 생성<br/>
- Login시 Game Data 로딩<br/>
- DBManager를 MysqlManager와 RedisManager로 분리<br/>
- 쿼리 처리는 Controller가 아닌 MysqlManager 내부에서 처리<br/>
- Name Rule에 안맞는 이름들 변경<br/><br/>

😀 감사합니다 😀      
