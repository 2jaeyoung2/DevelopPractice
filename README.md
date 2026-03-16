# DevelopPractice

practice whatever I want

## Player Movement (Jump)

- **Script**: `Assets/01. Scripts/PlayerMovement.cs`
- **Jump Buffer**
  - 점프 입력을 일정 시간(`jumpBufferTime`) 동안 저장해두고, 그 시간 안에 착지하면 점프가 발동되게 처리
  - **Key idea**: `lastJumpPressedTime`에 입력 시각 저장 → `Jump()`에서 버퍼 시간 안인지 체크
- **Coyote Time**
  - 발판에서 떨어진 직후에도 일정 시간(`coyoteTime`) 동안 점프를 허용
  - **Key idea**: `lastGroundedTime`에 마지막 지면 시각 저장 → `Jump()`에서 코요테 시간 안인지 체크

> Note: 위 기능들은 Unity Input System의 `Player Input (Invoke Unity Events)` 흐름을 기준으로 구현/테스트 중입니다.
