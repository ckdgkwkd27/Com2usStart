## 로그인 회원가입 이후 플레이어 데이터 생성 및 로딩(22.02.15)

### 사용법
`http://localhost:7038/Join: (회원가입) _ID, _Password에 계정정보를 전달하고 DB에 저장합니다.`   
`http://localhost:7038/Login: (로그인)  _ID, _Password에 계정정보를 전달하고 DB에서 계정이 유효한지 확인합니다.`                                                                       
<br/>

![ERD](https://user-images.githubusercontent.com/30414979/154002511-7fa5514e-5fb1-4a5c-a280-5381e714f9f9.png)


### 추가된 내용(22.02.14)
- 의미적으로 클래스 분리<br/>
- DB 패스워드 암호화하여 저장(Salt를 붙히고 SHA-256 해싱)<br/>
- ZLogger로 로그 출력
- 로그인시 인증토큰 부여, Redis로 관리<br/><br/>

😀 감사합니다 😀      
