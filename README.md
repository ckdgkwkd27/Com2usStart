## Dapper, ASP.NET Core Web API를 이용한 로그인, 회원가입 구현(22.02.14)

### 사용법
`http://localhost:7038/Join: (회원가입) _ID, _Password에 계정정보를 전달하고 DB에 저장합니다.`   
`http://localhost:7038/Login: (로그인)  _ID, _Password에 계정정보를 전달하고 DB에서 계정이 유효한지 확인합니다.`                                                                       
<br/>

### 추가된 내용(22.02.14)
- 의미적으로 클래스 분리<br/>
- DB 패스워드 암호화하여 저장(Salt를 붙히고 SHA-256 해싱)<br/>
- 로그인시 인증토큰 부여, Redis로 관리<br/><br/>

😀 감사합니다 😀      
